namespace Repos.ViewModels.AuthVM
{
    public class PostSignUpVM
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string ConfirmedPassword { get; set; }
    }
}
