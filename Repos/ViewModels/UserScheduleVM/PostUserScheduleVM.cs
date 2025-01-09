using System.ComponentModel.DataAnnotations;

namespace Repos.ViewModels.UserScheduleVM
{
    public class PostUserScheduleVM
    {
        [Required (ErrorMessage = "Vui lòng chọn người dùng!")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn lịch làm việc!")]
        public string ScheduleId { get; set; } = string.Empty;
    }
}
