using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace YKT.CORE.Services.Implementations
{
    public class AuthorizeTokenFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Verificar si la acción permite el acceso sin token
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (!ShouldAllowAnonymous(context))
            {
                // Verificar si hay un token de autorización en el encabezado
                return;
                if (string.IsNullOrWhiteSpace(authorizationHeader))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }

        private bool ShouldAllowAnonymous(AuthorizationFilterContext context)
        {
            // Obtener el nombre del controlador y la acción actual
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["page"]?.ToString();

            // Excluir ciertas acciones del filtro de autorización
            if (actionName == "/Index")
            {
                return true;
            }

            // Agregar más condiciones aquí para otras acciones que no requieren token de autorización

            return false;
        }
    }
}
