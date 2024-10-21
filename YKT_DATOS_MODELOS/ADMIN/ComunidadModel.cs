using YKT.ENTIDAD.Modelo.Auditoria;

namespace YKT_DATOS_MODELOS.ADMIN
{
    public class ComunidadModel : EntidadAuditoria
    {
        public string? TITULO { get; set; } = null;
        public string? DESCRIPCION { get; set; } = null;
        public DateTime? FINICIO { get; set; } = null;
        public DateTime? FFIN { get; set; } = null;
        public string? RUTA { get; set; } = null;
    }

    public class ComentarioModel : EntidadAuditoria
    {
        public int? IDUSUARIO { get; set; } = null;
        public int? IDFORO { get; set; } = null;
        public string? TEXTO { get; set; } = null;
        public string? DUSUARIO { get; set; } = null;
        public string? DRUTA { get; set; } = null;
        public string? DFORO { get; set; } = null;

    }
}
