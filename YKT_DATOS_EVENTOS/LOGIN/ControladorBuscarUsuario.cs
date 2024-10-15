using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using YKT.CORE.Structs;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using System.Text.Json;
using YKT_DATOS_EVENTOS.COMANDOS.LOGIN;
using YKT_DATOS_MODELOS.LOGIN;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YKT.CONFIG;

namespace YKT_DATOS_EVENTOS.LOGIN
{
    public class ControladorBuscarUsuario : IRequestHandler<ComandoBuscarUsuario, RespuestaConsulta>
    {
        private readonly IConfiguration _configuration;

        public ControladorBuscarUsuario(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespuestaConsulta> Handle(ComandoBuscarUsuario entidad, CancellationToken cancellationToken)
        {
            var parametros = new DynamicParameters();
            var json = JsonSerializer.Serialize(new
            {
                ID = entidad.ID,
                USUARIO = entidad.USUARIO,
                PASSWORD = entidad.PASSWORD,
                ROWS = 1,
                INIT = 0,
                CESTDO = 'A'
            });

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", null);
            parametros.Add("@p_nTipo", 4);
            parametros.Add("@p_nId", 0);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            var result = await FuncionesSql.EjecutarProcedimiento<UsuarioModel>(conexionSql, Procedimientos.SEGURIDAD.CrudRegister, parametros);
            var respuesta = new RespuestaConsulta();

            if(result.Count() == 0)
            {
                respuesta.CodEstado = -1;
                respuesta.Message = "El usuario no existe";
                return respuesta;
            }


            // validar si las contraseñas coinciden
            var primero = result.FirstOrDefault();
            if(primero.PASSWORD == entidad.PASSWORD)
            {
                respuesta.CodEstado = 1;
                respuesta.Message = "Usuario correcto";
                await GenerateToken(primero, respuesta);
                return respuesta;
            }

            respuesta.CodEstado = -1;
            respuesta.Message = "La contraseña es incorrecta";
            return respuesta;

        }

        private async Task GenerateToken(UsuarioModel user, RespuestaConsulta identity)
        {
            var secretKey = ConfiguracionProyecto.CAPTCHA.SecretKey;
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.USUARIO),
                new Claim("Rol", user.ROL),
                new Claim(ClaimTypes.Surname, (user.NOMBRES + " " + user.APELLIDOS))
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            identity.AccessToken = tokenHandler.WriteToken(createdToken);
        }

    }
}