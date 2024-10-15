using Dapper;
using Microsoft.Extensions.Configuration;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_MODELOS.ADMIN;
using System.Text.Json;

namespace YKT_DATOS_CONSULTAS.ADMIN
{
    public interface IConsultasRestaurant
    {
        Task<List<RestaurantModel>> Listar(RestaurantModel custom);

    }
    public class ConsultasRestaurant : IConsultasRestaurant
    {
        private readonly IConfiguration _configuration;

        public ConsultasRestaurant(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<RestaurantModel>> Listar(RestaurantModel custom)
        {
            var parametros = new DynamicParameters();

            var json = JsonSerializer.Serialize(new
            {
                DESC = custom.DESC,
                ESTADO = custom.ESTADO,
                INIT = custom.INIT,
                ROWS = custom.ROWS
            }).ToUpper();

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", null);
            parametros.Add("@p_nTipo", 4);
            parametros.Add("@p_nId", custom.ID ?? 0);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento<RestaurantModel>(conexionSql, Procedimientos.ADMIN.CrudRestaurant, parametros);
        }
     

    }
}

