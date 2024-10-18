using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using YKT.CORE.Structs;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using System.Text.Json;
using YKT_DATOS_EVENTOS.COMANDOS.ADM.USUARIO;

namespace YKT_DATOS_EVENTOS.ADMIN.USUARIO
{
    public class ControladorEditarUsuario : IRequestHandler<ComandoEditarUsuario, RespuestaConsulta>
    {
        private readonly IConfiguration _configuration;

        public ControladorEditarUsuario(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespuestaConsulta> Handle(ComandoEditarUsuario entidad, CancellationToken cancellationToken)
        {
            var parametros = new DynamicParameters();
            var json = JsonSerializer.Serialize(new
            {
                ID = entidad.ID,
                NOMBRES = entidad.NOMBRES,
                APELLIDOS = entidad.APELLIDOS,
                USUARIO = entidad.USUARIOADM,
                CORREO = entidad.CORREO,
                ROL = entidad.ROL,
                TELEFONO = entidad.TELEFONO,
                PASSWORD = entidad.PASSWORD,
                RUTA = entidad.RUTA,
                ESTADO = entidad.ESTADO
            });

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", entidad.USUARIO);
            parametros.Add("@p_nTipo", 1);
            parametros.Add("@p_nId", entidad.ID);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento(conexionSql, Procedimientos.ADMIN.CrudUsuario, parametros);
        }
    }
}
