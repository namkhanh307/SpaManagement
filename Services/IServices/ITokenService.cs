
using Repos.Entities;
using Repos.ViewModels.AuthVM;

namespace Services.IServices
{
    public interface ITokenService
    {
        GetTokensVM GenerateTokens(User user);
    }
}
