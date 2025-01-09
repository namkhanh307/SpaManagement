using Repos.Entities;

namespace Repos.ViewModels.UserScheduleVM
{
    public class GetUserScheduleVM : BaseVM
    {
        public string UserFullName { get; set; } = string.Empty;
        public DateTime ScheduleDate { get; set; }
        public TimeOnly ScheduleStartTime { get; set; }
        public TimeOnly ScheduleEndTime { get; set; }
        public string SalaryId { get; set; } = string.Empty;
    }
}
