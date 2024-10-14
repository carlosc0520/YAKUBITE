using YKT.CONFIG;
using YKT.CORE.Helpers;
using YKT.CORE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;

namespace YKT.CORE.Extensions
{
    public static class ConectAccessToken
    {
        public static async Task ConectarConAccessToken(HttpContext httpContext, string access_token, string refresh_token, IMemoryCache cache = null)
        {
            var token = access_token?.Split('.');
            var refreshtoken = refresh_token?.Split('.');

            //if (token == null || token.Length == 0)
            //    throw new Exception("sdasdsafasfaf");

            var base64Content = Base64Converter.GetBase64Content(token);
            var base64Contentrefresh = Base64Converter.GetBase64Content(refreshtoken);
            var user = JsonSerializer.Deserialize<AccessTokenUserInformation>(base64Content);
            var userrefresh = JsonSerializer.Deserialize<AccessTokenUserInformation>(base64Contentrefresh);
            var claims = GetClaims(access_token, refresh_token, user ?? userrefresh);

            var claimsIdentity = new ClaimsIdentity(
                 claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow.AddMinutes(ConfiguracionProyecto.TIEMPO_SESION_MINUTOS),
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(ConfiguracionProyecto.TIEMPO_SESION_MINUTOS)
            };

            await httpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 new ClaimsPrincipal(claimsIdentity),
                 authProperties);

            if (cache != null)
            {
                // Look for cache key.
                var cacheEntry = "";
                if (!cache.TryGetValue("ScrslSelec", out cacheEntry))
                {
                    // Key not in cache, so get data.
                    cacheEntry = user.ScrslSelec;

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromDays(1));

                    // Save data in cache.
                    cache.Set("ScrslSelec", cacheEntry, cacheEntryOptions);
                }
            }
        }
        public static List<Claim> GetClaims(string access_token, string refresh_token, AccessTokenUserInformation user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.nameid),
                new Claim(ClaimTypes.Name, user.unique_name),
                new Claim(ClaimTypes.Surname, user.family_name??""),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("access_token", access_token),
                new Claim("refresh_token", refresh_token),
                new Claim("SCRSLES", user.SCRSLES??""),
                new Claim("PRFLS", user.PRFLS??""),
                new Claim("ScrslSelec", user.ScrslSelec??""),
                new Claim("ActDirec",user.ActDirec),
                new Claim("USERID", user.USERID.ToString()),
                new Claim("WORKERID", user.WORKERID.ToString()),
                new Claim("IDCONTRATISTA", user.IDCONTRATISTA.ToString()),
                new Claim("FNCNRO", user.FNCNRO.ToString()),
            };
        }
    }
}
