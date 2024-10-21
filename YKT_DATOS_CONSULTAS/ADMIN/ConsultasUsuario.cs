using Dapper;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_MODELOS.ADMIN;

namespace YKT_DATOS_CONSULTAS.ADMIN
{
    public interface IConsultasUsuario
    {
        Task<List<UsuarioModel>> Listar(UsuarioModel custom);

    }
    public class ConsultasUsuario : IConsultasUsuario
    {
        private readonly IConfiguration _configuration;

        public ConsultasUsuario(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<UsuarioModel>> Listar(UsuarioModel custom)
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
            return await FuncionesSql.EjecutarProcedimiento<UsuarioModel>(conexionSql, Procedimientos.ADMIN.CrudUsuario, parametros);
        }
    }
}
