using ExBookWebAPI.Security;
using System.Security.Claims;
namespace ExBookWebAPI.Middlware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            (bool isValid, ClaimsPrincipal userClaim) = AuthenticationTokenManager.ValidateToken(token);
            if (!isValid)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid");
            }
            else
            {
                string? username = userClaim?.FindFirst(ClaimTypes.Name)?.Value;
                int user_id = Convert.ToInt32( userClaim?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                string id = userClaim?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? email = userClaim?.FindFirst(ClaimTypes.Email)?.Value;
                //string? phoneNumber = userClaim?.FindFirst(ClaimTypes.MobilePhone).Value;
                context.Items["Username"] = username;
                context.Items["User_id"] = user_id;
                context.Items["Email"] = email;

                await _next.Invoke(context);
            }
        }
    }
}
