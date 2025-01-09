namespace Repos.ViewModels.ScheduleVM
{
    public class GetScheduleVM : BaseVM
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
