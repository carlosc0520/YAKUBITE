using YKT.ENTIDAD.Modelo.Auditoria;

namespace YKT_DATOS_MODELOS.LOGIN
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

    }

    public class GDModel : EntidadAuditoria
    {
        public string? GD { get; set; } = null;
        public string? LABEL { get; set; } = null;
        public string? VALUE { get; set; } = null;

    }
}
