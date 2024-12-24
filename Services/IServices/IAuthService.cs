using Repos.ViewModels.AuthVM;
using Repos.ViewModels.UserVM;

namespace Services.IServices
{
    public interface IAuthService
    {
        Task<GetUsersVM> GetInfo();
        Task<GetSignInVM> SignIn(PostSignInVM model);
        Task SignUp(PostSignUpVM model);
    }
}
