using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.AuthVM
{
    public class PostSignUpVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên!")]
        public required string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu!")]
        public required string ConfirmedPassword { get; set; }
    }
}
