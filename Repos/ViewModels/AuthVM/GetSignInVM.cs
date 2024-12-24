using Repos.ViewModels.UserVM;

namespace Repos.ViewModels.AuthVM
{
    public class GetSignInVM
    {
        public GetUsersVM User { get; set; } = new GetUsersVM();
        public GetTokensVM Token { get; set; } = new GetTokensVM();
    }
}
