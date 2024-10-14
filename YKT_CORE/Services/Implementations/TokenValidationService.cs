using System.IdentityModel.Tokens.Jwt;
using System.Text;
using YKT.CONFIG;
using Microsoft.IdentityModel.Tokens;

public interface ITokenValidationService
{
    Task<bool> IsTokenValid(string token);
}

public class TokenValidationService : ITokenValidationService
{

    public TokenValidationService()
    {

    }

    public async Task<bool> IsTokenValid(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false; 
        }

        try
        {
            var secretKey = ConfiguracionProyecto.CAPTCHA.SecretKey;
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken validatedToken;
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            return true; // El token es válido
        }
        catch
        {
            return false; // Error al validar el token
        }
    }
}
