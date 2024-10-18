using MediatR;
using Microsoft.AspNetCore.Http;
using YKT.CORE.Structs;
using YKT.ENTIDAD.Modelo.Auditoria;

namespace YKT_DATOS_EVENTOS.COMANDOS.ADM.RESTAURANT
{
    public class ComandoInsertarMenu : EntidadAuditoria, IRequest<RespuestaConsulta>
    {
        public int? IDREST { get; set; } = null;
        public string? NOMBRE { get; set; } = null;
        public string? DESCRIPCION { get; set; } = null;
        public string? CATEGORIAMENU { get; set; } = null;
        public double? PRECIO { get; set; } = null;
        public int? STOCK { get; set; } = null;
        public string? RUTA { get; set; } = null;
        public IFormFile? FILE { get; set; } = null;
    }
}
