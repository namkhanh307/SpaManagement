using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.ScheduleVM
{
    public class PostScheduleVM
    {
        [Required(ErrorMessage = "Vui lòng nhập ngày!")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giờ bắt đầu!")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giờ kết thúc!")]
        public TimeOnly EndTime { get; set; }
    }
}
