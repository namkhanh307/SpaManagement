using Repos.ViewModels.AuthVM;
using System.Security.Claims;

namespace Services.IServices
{
    public interface ITokenService
    {
        Task<GetTokensVM> GenerateTokens(string userId, DateTime? expiredTime);
        Task<GetTokensVM> RefreshToken(string oldRT);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsTokenExpired(string token);
    }
}
