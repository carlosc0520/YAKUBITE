using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace YKT.CORE.Helpers
{
    public class HttpContextDraw
    {
        public static void SetModelValues<T>(HttpContext context, T miModelo)
        {
            string draw = context.Request.Query["draw"].ToString();
            int start = Convert.ToInt32(context.Request.Query["start"]);
            int length = Convert.ToInt32(context.Request.Query["length"]);
            string searchTerm = context.Request.Query["search[value]"].ToString();

            var drawProp = typeof(T).GetProperty("DRAW");
            if (drawProp != null) drawProp.SetValue(miModelo, draw);

            var initProp = typeof(T).GetProperty("INIT");
            if (initProp != null) initProp.SetValue(miModelo, start);

            var rowsProp = typeof(T).GetProperty("ROWS");
            if (rowsProp != null) rowsProp.SetValue(miModelo, length == -1 ? 10000 : length);

            var descProp = typeof(T).GetProperty("DESC");
            if (descProp != null) descProp.SetValue(miModelo, searchTerm);
        }


        public static string User(HttpContext httpContext, int? opcion = 1)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                throw new ArgumentException("El encabezado de autorización es nulo o vacío.", nameof(authorizationHeader));
            }

            var token = authorizationHeader.Split(' ').Last();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("No se encontró un token de autorización válido en el encabezado.", nameof(authorizationHeader));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var filtro = "";

            switch (opcion)
            {
                case 1:
                    filtro = "unique_name"; break;
                case 2:
                    filtro = "nameid"; break;
                case 3:
                    filtro = "family_name"; break;
                case 4:
                    filtro = "email"; break;
                case 5:
                    filtro = "IDMRCA"; break;
                default:
                    return string.Empty;
            }

            var userNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == filtro);
            if (userNameClaim != null)
            {
                return userNameClaim.Value;
            }

            throw new InvalidOperationException("No se encontró el claim 'Name' en el token de autorización.");
        }
    }
}
