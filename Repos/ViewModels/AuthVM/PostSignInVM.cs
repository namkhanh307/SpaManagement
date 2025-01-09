using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.AuthVM
{
    public class PostSignInVM
    {
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public required string Password { get; set; }
    }
}
