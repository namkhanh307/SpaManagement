using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.UserVM
{
    public class PostUserVM
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên!")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập vai trò!")]
        public string RoleFullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { get; set; } = string.Empty;
    }
}
