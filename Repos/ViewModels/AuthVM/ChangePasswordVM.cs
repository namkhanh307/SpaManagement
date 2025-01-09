using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.AuthVM
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ!")]
        public required string OldPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        public required string NewPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu!")]
        public required string ConfirmPassword { get; set; }

    }
}
