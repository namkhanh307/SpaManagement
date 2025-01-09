using Repos.ViewModels.OrderDetailVM;
using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.OrderVM
{
    public class PostOrderVM
    {
        [Required (ErrorMessage = "Vui lòng nhập tên!")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn người dùng!")]
        public required string UserId { get; set; }

        public ICollection<PostOrderDetailVM> OrderDetails { get; set; } = [];
    }
}
