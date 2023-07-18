namespace WebStoreApi.Authorization
{
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Security.Cryptography;
    using WebStoreApi.Helpers;

    public interface IJwtUtils
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        ClaimsPrincipal GetClaimsPrincipal(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly WebStoreAppSettings _appSettings;

        public JwtUtils(IOptions<WebStoreAppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_appSettings.Secret)
            );
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:7296/",
                audience: "https://localhost:7296/s",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: signinCredentials
             );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var tokenValidatorParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_appSettings.Secret)
                ),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidatorParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
