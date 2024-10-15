using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using YKT.CORE.Structs;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_EVENTOS.COMANDOS.ADM.RESTAURANT;
using System.Text.Json;

namespace YKT_DATOS_EVENTOS.ADMIN.RESTAURANT
{
    internal class ControladorEditarRestaurant : IRequestHandler<ComandoEditarRestaurant, RespuestaConsulta>
    {
        private readonly IConfiguration _configuration;

        public ControladorEditarRestaurant(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespuestaConsulta> Handle(ComandoEditarRestaurant entidad, CancellationToken cancellationToken)
        {
            var parametros = new DynamicParameters();
            var json = JsonSerializer.Serialize(new
            {
                ID = entidad.ID,
                RUC = entidad.RUC,
                DESCRIPCION = entidad.DESCRIPCION,
                ALIAS = entidad.ALIAS,
                RUTA = entidad.RUTA,
                DIRECCION = entidad.DIRECCION,
                CATEGORIAGD = entidad.CATEGORIAGD,
                ESTADO = entidad.ESTADO
            });

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", entidad.USUARIO);
            parametros.Add("@p_nTipo", 1);
            parametros.Add("@p_nId", entidad.ID);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento(conexionSql, Procedimientos.ADMIN.CrudRestaurant, parametros);
        }
    }
}
