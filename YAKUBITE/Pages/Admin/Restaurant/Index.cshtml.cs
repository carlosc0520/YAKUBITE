using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using YKT.CONFIG;
using YKT.CORE.Helpers;
using YKT_DATOS_CONSULTAS.ADMIN;
using YKT_DATOS_EVENTOS.COMANDOS.ADM.RESTAURANT;
using YKT_DATOS_MODELOS.ADMIN;

namespace YAKUBITE.Pages.Admin.Restaurant
{
  [IgnoreAntiforgeryToken(Order = 1001)]

  public class IndexModel : PageModel
  {
    private readonly IMediator _mediator;
    private readonly IConsultasRestaurant _consultasRestaurant;

    public IndexModel(
      IConsultasRestaurant consultasRestaurant,
      IMediator mediator
    )
    {
      _consultasRestaurant = consultasRestaurant;
      _mediator = mediator;
    }

    #region RESTAURANT
    [HttpGet]
    public async Task<IActionResult> OnGetBuscarAsync([FromQuery] RestaurantModel custom)
    {
      try
      {
        HttpContextDraw.SetModelValues(HttpContext, custom);
        var datos = await _consultasRestaurant.Listar(custom);
        var totalRows = datos?.FirstOrDefault()?.TOTALROWS ?? 0;

        datos.ForEach(e =>
        {
          e.RUTA = Path.Combine(ConfiguracionProyecto.DISK, e.RUTA);
        });

        return new JsonResult(new { recordsTotal = totalRows, recordsFiltered = totalRows, data = datos, draw = custom.DRAW });
      }
      catch (Exception ex)
      {
        return BadRequest(new { success = false, message = "Ocurrió un error al listar los restaurantes.", error = ex.Message });
      }

    }

    [HttpPost]
    public async Task<IActionResult> OnPostAddAsync([FromForm] ComandoInsertarRestaurant comando)
    {
      try
      {
        if (comando.FILE != null && comando.FILE.Length > 0)
        {
          string folderPath = Path.Combine(ConfiguracionProyecto.DISK, "RESTAURANT");
          if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

          string filePath = Path.Combine(folderPath, comando.FILE.FileName);
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await comando.FILE.CopyToAsync(stream);
          }

          comando.RUTA = filePath.Replace(ConfiguracionProyecto.DISK, "");
        }

        comando.USUARIO = HttpContextDraw.User(HttpContext, 1);
        var result = await _mediator.Send(comando);

        return new JsonResult(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error interno del servidor: " + ex.Message);
      }
    }


    [HttpPost]
    public async Task<IActionResult> OnPostUpdateAsync([FromForm] ComandoEditarRestaurant comando)
    {
      try
      {
        if (comando.FILE != null && comando.FILE.Length > 0)
        {
          if (!string.IsNullOrEmpty(comando.RUTA))
          {
            if (System.IO.File.Exists(comando.RUTA)) System.IO.File.Delete(comando.RUTA);
          }

          string folderPath = Path.Combine(ConfiguracionProyecto.DISK, "RESTAURANT");
          if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

          string filePath = Path.Combine(folderPath, comando.FILE.FileName);
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await comando.FILE.CopyToAsync(stream);
          }

          comando.RUTA = filePath;
        }

        comando.RUTA = comando.RUTA.Replace(ConfiguracionProyecto.DISK, "");
        comando.USUARIO = HttpContextDraw.User(HttpContext, 1);
        var result = await _mediator.Send(comando);
        return new JsonResult(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, "Error interno del servidor: " + ex.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> OnPostDeleteAsync([FromForm] ComandoEliminarRestaurant comando)
    {
      comando.USUARIO = HttpContextDraw.User(HttpContext, 1);
      var result = await _mediator.Send(comando);
      return new JsonResult(result);
    }

    #endregion
  }
}
