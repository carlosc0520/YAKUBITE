using YKT.CORE.Structs;
using YKT.ENTIDAD.Modelo.Auditoria;
using MediatR;

namespace YKT_DATOS_EVENTOS.COMANDOS.LOGIN
{
    public class ComandoBuscarUsuario : EntidadAuditoria, IRequest<RespuestaConsulta>
    {
        public string? USUARIO { get; set; } = null;
        public string? PASSWORD { get; set; } = null;
    }
}
