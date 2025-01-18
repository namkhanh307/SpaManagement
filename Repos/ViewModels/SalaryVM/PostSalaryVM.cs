using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.SalaryVM
{
    public class PostSalaryVM
    {
        [Required(ErrorMessage = "Vui lòng nhập tháng!")]
        public required int Month { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập năm!")]
        public required int Year { get; set; }
        public string? UserId { get; set; }
    }
}
