using YKT.CORE.Structs;
using YKT.ENTIDAD.Modelo.Auditoria;
using MediatR;

namespace YKT_DATOS_EVENTOS.COMANDOS.REGISTER
{
    public class ComandoRegistrarUsuario : EntidadAuditoria, IRequest<RespuestaConsulta>
    {
        public string? NOMBRES  { get; set; } = null;
        public string? APELLIDOS { get; set; } = null;
        public string? USUARIO { get; set; } = null;
        public string? CORREO { get; set; } = null;
        public string? ROL { get; set; } = null;
        public string? TELEFONO { get; set; } = null;
        public string? PASSWORD { get; set; } = null;

    }
}
