using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.PayRateVM
{
    public class PostPayRateVM
    {
        [Required(ErrorMessage = "Vui lòng chọn người dùng!")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền!")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0!")]
        public double Amount { get; set; }
    }
}
