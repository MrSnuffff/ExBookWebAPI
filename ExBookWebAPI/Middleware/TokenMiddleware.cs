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
                string username = userClaim?.FindFirst(ClaimTypes.Name)?.Value;

                context.Items["Username"] = username;
                await _next.Invoke(context);
            }
        }
    }
}
