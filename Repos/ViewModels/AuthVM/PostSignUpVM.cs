namespace Repos.ViewModels.AuthVM
{
    public class PostSignUpVM
    {
        public required string Name { get; set; }
        public required string UserName { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public required string ConfirmedPassword { get; set; }
    }
}
