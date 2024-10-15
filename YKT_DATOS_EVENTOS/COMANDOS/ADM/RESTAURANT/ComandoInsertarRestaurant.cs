using YKT.CORE.Structs;
using YKT.ENTIDAD.Modelo.Auditoria;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace YKT_DATOS_EVENTOS.COMANDOS.ADM.RESTAURANT
{
    public class ComandoInsertarRestaurant : EntidadAuditoria, IRequest<RespuestaConsulta>
    {
        public string? RUC { get; set; } = null;
        public string? DESCRIPCION { get; set; } = null;
        public string? ALIAS { get; set; } = null;
        public string? RUTA { get; set; } = null;
        public string? DIRECCION { get; set; } = null;
        public string? CATEGORIAGD { get; set; } = null;
        public IFormFile? FILE { get; set; } = null;
    }
}
