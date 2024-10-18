using YKT.ENTIDAD.Modelo.Auditoria;

namespace YKT_DATOS_MODELOS.ADMIN
{
    public class RestaurantModel : EntidadAuditoria
    {
        public string? RUC { get; set; } = null;
        public string? DESCRIPCION { get; set; } = null;
        public string? ALIAS { get; set; } = null;
        public string? RUTA { get; set; } = null;
        public string? DIRECCION { get; set; } = null;
        public string? CATEGORIAGD { get; set; } = null;
        public string? DCATEGORIAGD { get; set; } = null;
    }

    public class MenuModel : EntidadAuditoria
    {
        public int? IDREST { get; set; } = null;
        public string? NOMBRE { get; set; } = null;
        public string? DESCRIPCION { get; set; } = null;
        public string? CATEGORIAMENU { get; set; } = null;
        public string? DCATEGORIAMENU { get; set; } = null;
        public double? PRECIO { get; set; } = null;
        public int? STOCK { get; set; } = null;
        public string? RUTA { get; set; } = null;
    }
}
