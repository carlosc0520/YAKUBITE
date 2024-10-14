using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using YKT.CORE.Structs;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_EVENTOS.COMANDOS.REGISTER;
using System.Text.Json;

namespace YKT_DATOS_EVENTOS.REGISTER
{
    public class ControladorRegistrarUsuario : IRequestHandler<ComandoRegistrarUsuario, RespuestaConsulta>
    {
        private readonly IConfiguration _configuration;

        public ControladorRegistrarUsuario(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespuestaConsulta> Handle(ComandoRegistrarUsuario entidad, CancellationToken cancellationToken)
        {
            var parametros = new DynamicParameters();
            var json = JsonSerializer.Serialize(new
            {
                ID = entidad.ID,
                NOMBRES = entidad.NOMBRES,
                APELLIDOS = entidad.APELLIDOS,
                USUARIO = entidad.USUARIO,
                CORREO = entidad.CORREO,
                ROL = entidad.ROL,
                TELEFONO = entidad.TELEFONO,
                PASSWORD = entidad.PASSWORD
            });

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", entidad.USUARIO);
            parametros.Add("@p_nTipo", 1);
            parametros.Add("@p_nId", 0);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento(conexionSql, Procedimientos.SEGURIDAD.CrudRegister, parametros);
        }
    }
}