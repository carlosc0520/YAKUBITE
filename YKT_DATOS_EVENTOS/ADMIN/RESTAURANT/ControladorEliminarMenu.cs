using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using YKT.CORE.Structs;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_EVENTOS.COMANDOS.ADM.RESTAURANT;

namespace YKT_DATOS_EVENTOS.ADMIN.RESTAURANT
{
    public class ControladorEliminarMenu : IRequestHandler<ComandoEliminarMenu, RespuestaConsulta>
    {
        private readonly IConfiguration _configuration;

        public ControladorEliminarMenu(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespuestaConsulta> Handle(ComandoEliminarMenu entidad, CancellationToken cancellationToken)
        {
            var parametros = new DynamicParameters();
            var json = JsonSerializer.Serialize(new
            {
                ID = entidad.ID
            });

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", entidad.USUARIO);
            parametros.Add("@p_nTipo", 2);
            parametros.Add("@p_nId", entidad.ID);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento(conexionSql, Procedimientos.ADMIN.CrudMenu, parametros);
        }
    }
}
