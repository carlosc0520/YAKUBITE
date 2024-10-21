using Dapper;
using Microsoft.Extensions.Configuration;
using YKT.DATABASE.Helper;
using YKT.DATABASE;
using YKT_DATOS_MODELOS.ADMIN;
using System.Text.Json;

namespace YKT_DATOS_CONSULTAS.ADMIN
{
    public interface IConsultasComunidad
    {
        Task<List<ComunidadModel>> Listar(ComunidadModel custom);
        Task<List<ComentarioModel>> ListarRespuesta(ComentarioModel custom);

    }
    public class ConsultasComunidad : IConsultasComunidad
    {
        private readonly IConfiguration _configuration;

        public ConsultasComunidad(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ComunidadModel>> Listar(ComunidadModel custom)
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
            return await FuncionesSql.EjecutarProcedimiento<ComunidadModel>(conexionSql, Procedimientos.ADMIN.CrudComunidad, parametros);
        }
        public async Task<List<ComentarioModel>> ListarRespuesta(ComentarioModel custom)
        {
            var parametros = new DynamicParameters();

            var json = JsonSerializer.Serialize(new
            {
                DESC = custom.DESC,
                ESTADO = custom.ESTADO,
                INIT = custom.INIT,
                ROWS = custom.ROWS,
                IDFORO = custom.IDFORO
            }).ToUpper();

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", null);
            parametros.Add("@p_nTipo", 4);
            parametros.Add("@p_nId", custom.ID ?? 0);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento<ComentarioModel>(conexionSql, Procedimientos.CLIENTE.CrudComentarios, parametros);
        }
    }
}

