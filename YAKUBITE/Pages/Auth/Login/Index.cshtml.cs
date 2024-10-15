using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YKT.CORE.Helpers;
using YKT_DATOS_CONSULTAS.LOGIN;
using YKT_DATOS_EVENTOS.COMANDOS.LOGIN;
using YKT_DATOS_MODELOS.LOGIN;

namespace YAKUBITE.Pages.Auth.Login
{
  [IgnoreAntiforgeryToken(Order = 1001)]

  public class IndexModel : PageModel
  {
    private readonly IMediator _mediator;
    private readonly IConsultasLogin _consultasLogin;

    public IndexModel(
      IConsultasLogin consultasLogin,
      IMediator mediator
    )
    {
      _consultasLogin = consultasLogin;
      _mediator = mediator;
    }

    #region RESTAURANT
    [HttpGet]
    public async Task<IActionResult> OnGetComboAsync([FromQuery] GDModel custom)
    {
      try
      {
        HttpContextDraw.SetModelValues(HttpContext, custom);
        var datos = await _consultasLogin.Listar(custom);
        var totalRows = datos?.FirstOrDefault()?.TOTALROWS ?? 0;
        return new JsonResult(new { recordsTotal = totalRows, data = datos });
      }
      catch (Exception ex)
      {
        return BadRequest(new { success = false, message = "Ocurrió un error al listar los restaurantes.", error = ex.Message });
      }

    }

    #endregion

    [HttpPost]
    public async Task<IActionResult> OnPostLoginAsync(ComandoBuscarUsuario comando)
    {
      var result = await _mediator.Send(comando);
      return new JsonResult(result);
    }
  }
}
