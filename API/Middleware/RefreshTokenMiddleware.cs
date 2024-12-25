using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repos.Entities;
using Repos.IRepos;
using Services.IServices;

namespace API.Middleware
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            // Create a new scope for scoped services
            using var scope = serviceProvider.CreateScope();

            // Resolve services within the scoped context
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader?.StartsWith("Bearer ") == true)
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Validate and process the token
                var principal = tokenService.GetPrincipalFromExpiredToken(token);
                if (principal != null)
                {
                    var userId = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

                    // Check if the token has expired
                    if (tokenService.IsTokenExpired(token))
                    {
                        var userToken = await unitOfWork.GetRepo<UserTokens>()
                            .Entities
                            .FirstOrDefaultAsync(ut => ut.UserId.ToString() == userId);

                        if (userToken != null && userToken.ExpiredTime > DateTime.UtcNow)
                        {
                            // Refresh tokens
                            var newAccessToken = await tokenService.GenerateTokens(userId, null);
                            var newRefreshToken = await tokenService.GenerateNewRefreshTokenAsync(userToken.RefreshToken);

                            // Add the new tokens to response headers
                            context.Response.Headers["New-Access-Token"] = newAccessToken.AccessToken;
                            context.Response.Headers["New-Refresh-Token"] = newRefreshToken.RefreshToken;

                            // Replace the user's principal in the HttpContext
                            context.User = principal;
                        }
                    }
                    else
                    {
                        // Token is still valid; set the user context
                        context.User = principal;
                    }
                }
            }

            await _next(context);
        }
    }


}
