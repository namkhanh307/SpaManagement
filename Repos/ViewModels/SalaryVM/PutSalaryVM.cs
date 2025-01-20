using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.SalaryVM
{
    public class PutSalaryVM
    {
        [Required(ErrorMessage = "Vui lòng nhập số tiền!")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0!")]
        public double Total { get; set; }
    }
}
