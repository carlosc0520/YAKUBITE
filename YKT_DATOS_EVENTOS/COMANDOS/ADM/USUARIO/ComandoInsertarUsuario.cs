using MediatR;
using Microsoft.AspNetCore.Http;
using YKT.CORE.Structs;
using YKT.ENTIDAD.Modelo.Auditoria;

namespace YKT_DATOS_EVENTOS.COMANDOS.ADM.USUARIO
{
    public class ComandoInsertarUsuario : EntidadAuditoria, IRequest<RespuestaConsulta>
    {
        public string? NOMBRES { get; set; } = null;
        public string? APELLIDOS { get; set; } = null;
        public string? CORREO { get; set; } = null;
        public string? ROL { get; set; } = null;
        public string? TELEFONO { get; set; } = null;
        public string? USUARIOADM { get; set; } = null;
        public string? PASSWORD { get; set; } = null;
        public string? RUTA { get; set; } = null;
        public IFormFile? FILE { get; set; } = null;
    }
}
