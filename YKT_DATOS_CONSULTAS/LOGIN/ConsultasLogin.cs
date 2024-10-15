using Dapper;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_MODELOS.LOGIN;

namespace YKT_DATOS_CONSULTAS.LOGIN
{
    public interface IConsultasLogin
    {
        Task<List<GDModel>> Listar(GDModel custom);

    }
    public class ConsultasLogin : IConsultasLogin
    {
        private readonly IConfiguration _configuration;

        public ConsultasLogin(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<GDModel>> Listar(GDModel custom)
        {
            var parametros = new DynamicParameters();

            var json = JsonSerializer.Serialize(new
            {
                DESC = custom.DESC,
                ESTADO = custom.ESTADO,
                INIT = custom.INIT,
                ROWS = custom.ROWS,
                AGRUPADOR = custom.GD
            }).ToUpper();

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", null);
            parametros.Add("@p_nTipo", 5);
            parametros.Add("@p_nId", custom.ID ?? 0);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento<GDModel>(conexionSql, Procedimientos.SEGURIDAD.CrudCombos, parametros);
        }


    }
}
