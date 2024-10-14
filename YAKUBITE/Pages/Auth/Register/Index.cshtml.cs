using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YKT_DATOS_EVENTOS.COMANDOS.REGISTER;

namespace YAKUBITE.Pages.Auth.Register
{
  [IgnoreAntiforgeryToken(Order = 1001)]
  public class IndexModel : PageModel
  {
    private readonly IMediator _mediator;

    public IndexModel(
      IMediator mediator
    )
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> OnPostRegisterAsync(ComandoRegistrarUsuario comando)
    {
      var result = await _mediator.Send(comando);
      return new JsonResult(result);
    }
  }
}
