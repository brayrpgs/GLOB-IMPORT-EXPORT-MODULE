using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace api.Middlewares
{
    public class RequestValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            if (path.StartsWith("/swagger") || path.StartsWith("/index.html"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Method == HttpMethods.Options)
            {
                await _next(context);
                return;
            }

            var origin = context.Request.Headers.Origin.ToString();
            var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost:5173";
            var apiUrlHttp = "http://localhost:5184";
            var apiUrlHttps = "https://localhost:5184";

            if (!string.IsNullOrEmpty(origin) &&
                origin != clientUrl &&
                origin != apiUrlHttp &&
                origin != apiUrlHttps)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "Forbidden",
                    StatusCode = 403,
                });
                return;
            }

            var authHeader = context.Request.Headers.Authorization.ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "Unauthorized",
                    StatusCode = 401,
                });
                return;
            }

            var token = authHeader.StartsWith("Bearer ")
                ? authHeader["Bearer ".Length..].Trim()
                : authHeader.Trim();

            try
            {
                var jwtSecret = Environment.GetEnvironmentVariable("AUTH_API_JWT_SECRET");
                if (string.IsNullOrEmpty(jwtSecret))
                    throw new Exception("JWT secret not configured");

                var key = Encoding.UTF8.GetBytes(jwtSecret);
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch (SecurityTokenExpiredException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "Unauthorized",
                    StatusCode = 401,
                });
                return;
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    Title = "Unauthorized",
                    StatusCode = 401,
                });
                return;
            }

            await _next(context);
        }
    }
}
