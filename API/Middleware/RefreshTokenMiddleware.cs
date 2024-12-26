using Microsoft.EntityFrameworkCore;
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
            using var scope = serviceProvider.CreateScope();

            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader?.StartsWith("Bearer ") == true)
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var principal = tokenService.GetPrincipalFromExpiredToken(token);
                if (principal != null)
                {
                    string? userId = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "";
                    //var userId = Authentication.GetUserIdFromHttpContextAccessor(_contextAccessor)
                    if (tokenService.IsTokenExpired(token))
                    {
                        var userToken = await unitOfWork.GetRepo<UserTokens>()
                            .Entities
                            .FirstOrDefaultAsync(ut => ut.UserId.ToString() == userId);

                        if (userToken != null && userToken.ExpiredTime > DateTime.UtcNow)
                        {
                            var newAccessToken = await tokenService.GenerateTokens(userId, null);
                            var newRefreshToken = await tokenService.GenerateNewRefreshTokenAsync(userToken.RefreshToken);

                            context.Response.Headers["New-Access-Token"] = newAccessToken.AccessToken;
                            context.Response.Headers["New-Refresh-Token"] = newRefreshToken.RefreshToken;
                            context.User = principal;
                        }
                    }
                    else
                    {
                        context.User = principal;
                    }
                }
            }

            await _next(context);
        }
    }


}
