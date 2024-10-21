using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using YKT.CONFIG;
using YKT.CORE.Helpers;
using YKT_DATOS_CONSULTAS.ADMIN;
using YKT_DATOS_EVENTOS.COMANDOS.ADM.USUARIO;
using YKT_DATOS_MODELOS.ADMIN;

namespace YAKUBITE.Pages.Admin.Usuarios
{
  [IgnoreAntiforgeryToken(Order = 1001)]

  public class IndexModel : PageModel
  {
    private readonly IMediator _mediator;
    private readonly IConsultasUsuario _consultasUsuario;

    public IndexModel(
      IConsultasUsuario consultasUsuario,
      IMediator mediator
    )
    {
      _consultasUsuario = consultasUsuario;
      _mediator = mediator;
    }

    #region RESTAURANT
    [HttpGet]
    public async Task<IActionResult> OnGetBuscarAsync([FromQuery] UsuarioModel custom)
    {
      try
      {
        HttpContextDraw.SetModelValues(HttpContext, custom);
        var datos = await _consultasUsuario.Listar(custom);
        var totalRows = datos?.FirstOrDefault()?.TOTALROWS ?? 0;

        datos.ForEach(e =>
        {
          if(!e.RUTA.IsNullOrEmpty()) e.RUTA = Path.Combine(ConfiguracionProyecto.HOST, e.RUTA.Replace("\\", "/"));
        });

        return new JsonResult(new { recordsTotal = totalRows, recordsFiltered = totalRows, data = datos, draw = custom.DRAW });
      }
      catch (Exception ex)
      {
        return BadRequest(new { success = false, message = "Ocurrió un error al listar los usuarios.", error = ex.Message });
      }

    }


    [HttpPost]
    public async Task<IActionResult> OnPostAddAsync([FromForm] ComandoInsertarUsuario comando)
    {
      try
      {
        if (comando.FILE != null && comando.FILE.Length > 0)
        {
          string folderPath = Path.Combine(ConfiguracionProyecto.DISK, "USUARIOS");
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
    public async Task<IActionResult> OnPostUpdateAsync([FromForm] ComandoEditarUsuario comando)
    {
      try
      {
        if (comando.FILE != null && comando.FILE.Length > 0)
        {
          if (!string.IsNullOrEmpty(comando.RUTA))
          {
            if (System.IO.File.Exists(comando.RUTA)) System.IO.File.Delete(comando.RUTA);
          }

          string folderPath = Path.Combine(ConfiguracionProyecto.DISK, "USUARIOS");
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
    public async Task<IActionResult> OnPostDeleteAsync([FromForm] ComandoEliminarUsuario comando)
    {
      comando.USUARIO = HttpContextDraw.User(HttpContext, 1);
      var result = await _mediator.Send(comando);
      return new JsonResult(result);
    }

    #endregion


  }
}
