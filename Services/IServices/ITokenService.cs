
using Repos.Entities;
using Repos.ViewModels.AuthVM;
using System.Security.Claims;

namespace Services.IServices
{
    public interface ITokenService
    {
        Task<GetTokensVM> GenerateTokens(string userId, DateTime? expiredTime);
        Task<GetTokensVM> GenerateNewRefreshTokenAsync(string oldRefreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsTokenExpired(string token);
    }
}
