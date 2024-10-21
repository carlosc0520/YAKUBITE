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
    public class ControladorEditarMenu : IRequestHandler<ComandoEditarMenu, RespuestaConsulta>
    {
        private readonly IConfiguration _configuration;

        public ControladorEditarMenu(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<RespuestaConsulta> Handle(ComandoEditarMenu entidad, CancellationToken cancellationToken)
        {
            var parametros = new DynamicParameters();
            var json = JsonSerializer.Serialize(new
            {
                ID = entidad.ID,
                IDREST = entidad.IDREST,
                NOMBRE = entidad.NOMBRE,
                DESCRIPCION = entidad.DESCRIPCION,
                CATEGORIAMENU = entidad.CATEGORIAMENU,
                PRECIO = entidad.PRECIO,
                RUTA = entidad.RUTA,
                STOCK = entidad.STOCK,
                ESTADO = entidad.ESTADO
            });

            parametros.Add("@p_cData", json);
            parametros.Add("@p_cUser", entidad.USUARIO);
            parametros.Add("@p_nTipo", 1);
            parametros.Add("@p_nId", entidad.ID);

            var conexionSql = _configuration.GetConnectionString("DefaultConnection");
            return await FuncionesSql.EjecutarProcedimiento(conexionSql, Procedimientos.ADMIN.CrudMenu, parametros);
        }
    }
}
