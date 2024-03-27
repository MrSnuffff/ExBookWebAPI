using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.Security
{
    public class AuthenticationTokenManager
    {
        private const string Secret = "0u2efh91-742839120473829@#$%^8&^%$fgufiyq2uiur####u2eofeyyv2yvuf2yvuovu674jhgvgvVGVGVGVGGVGGLASVEGAS@##@#@@##@#@@##@______87687687766676787887878464LKLKLKLHHUUHHUUuhuhjhj;hbhcadfsayfsggasddaf";
        private const int AccessTokenExpirationMinutes = 60 * 24; // 1 day
        private const int RefreshTokenExpirationMinutes = 60 * 24 * 30; // 30 days

        public static (string accessToken, string refreshToken) GenerateTokens(string username, string email, string phone_number, string user_id)
        {
            var symmetricKey = Encoding.UTF8.GetBytes(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.MobilePhone, phone_number),
                new Claim(ClaimTypes.NameIdentifier, user_id)
            });
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };


            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims, // Включаем Subject для refreshToken
                Expires = DateTime.UtcNow.AddMinutes(RefreshTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return (tokenHandler.WriteToken(accessToken), tokenHandler.WriteToken(refreshToken));
        }



        public static (bool isValid, ClaimsPrincipal principal) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Secret);

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);

                return (true, principal as ClaimsPrincipal);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

    }

}
