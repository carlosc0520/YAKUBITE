using MediatR;
using YKT.CORE.Structs;

namespace YKT_DATOS_EVENTOS.COMANDOS.ADM.USUARIO
{
    public class ComandoEliminarUsuario : IRequest<RespuestaConsulta>
    {
        public int? ID { get; set; } = null;
        public string? USUARIO { get; set; } = null;
    }
}
