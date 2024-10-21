using YKT.ENTIDAD.Modelo.Auditoria;

namespace YKT_DATOS_MODELOS.ADMIN
{
    public class UsuarioModel : EntidadAuditoria
    {
        public string? NOMBRES { get; set; } = null;
        public string? APELLIDOS { get; set; } = null;
        public string? CORREO { get; set; } = null;
        public string? ROL { get; set; } = null;
        public string? DROL { get; set; } = null;
        public string? TELEFONO { get; set; } = null;
        public string? PASSWORD { get; set; } = null;
        public string? RUTA { get; set; } = null;

    }
}
