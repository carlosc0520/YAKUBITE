using System.Security.Claims;

namespace YKT.CORE.Extensions
{
    public static class ClaimsExtension
    {
        public static string GetFullName(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user?.Claims?.FirstOrDefault(v => v.Type == ClaimTypes.Surname)?.Value ?? "-";
            }

            return "-";
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return Convert.ToInt32(user?.Claims?.FirstOrDefault(v => v.Type == "USERID")?.Value ?? "0");
            }

            return 0;
        }
        public static int GetWorkerId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var id = user?.Claims?.FirstOrDefault(v => v.Type == "WORKERID")?.Value ?? "0";
                return Convert.ToInt32(id);
            }

            return 0;
        }
        public static int? GetContratistaId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var id = user?.Claims?.FirstOrDefault(v => v.Type == "IDCONTRATISTA")?.Value ?? "0";
                return Convert.ToInt32(id);
            }

            return 0;
        }
        public static string GetUserCode(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                if (user.EstaAutenticadoPorActiveDirectory())
                {
                    return user?.Claims?.FirstOrDefault(v => v.Type == ClaimTypes.Email)?.Value ?? "-";
                }
                return user?.Claims?.FirstOrDefault(v => v.Type == ClaimTypes.Name)?.Value ?? "-";
            }

            return "-";
        }
        public static string GetUserAccessToken(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user?.Claims?.FirstOrDefault(v => v.Type == "access_token")?.Value ?? "";
            }
            return "";
        }
        public static string GetUserRefreshToken(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user?.Claims?.FirstOrDefault(v => v.Type == "refresh_token")?.Value ?? "";
            }
            return "";
        }
        public static string ObtenerSucursales(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var sucursales = user?.Claims?.FirstOrDefault(v => v.Type == "SCRSLES")?.Value ?? "";
                return sucursales;
            }
            return null;
        }
        public static string ObtenerPerfiles(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var sucursales = user?.Claims?.FirstOrDefault(v => v.Type == "PRFLS")?.Value ?? "";
                return sucursales;
            }
            throw new Exception("Usuario no logueado");
        }
        public static bool EsAdmin(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var prfles = user?.Claims?.FirstOrDefault(v => v.Type == "PRFLS")?.Value ?? "";
                var perfiles = prfles.Split(",");
                return perfiles.Contains("1");
            }
            throw new Exception("Usuario no logueado");
        }
        public static bool EsFuncionario(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var sucursales = user?.Claims?.FirstOrDefault(v => v.Type == "FNCNRO")?.Value ?? "0";
                var esFunc = Convert.ToBoolean(Convert.ToInt16(sucursales));
                return esFunc;
            }
            throw new Exception("Usuario no logueado");
        }
        public static bool EstaAutenticadoPorActiveDirectory(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var sucursales = user?.Claims?.FirstOrDefault(v => v.Type == "ActDirec")?.Value ?? "";

                return (!string.IsNullOrEmpty(sucursales) ? Convert.ToBoolean(sucursales) : false);
            }
            throw new Exception("Usuario no logueado");
        }
    }
}
