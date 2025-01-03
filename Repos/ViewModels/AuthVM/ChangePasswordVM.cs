namespace Repos.ViewModels.AuthVM
{
    public class ChangePasswordVM
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }

    }
}
