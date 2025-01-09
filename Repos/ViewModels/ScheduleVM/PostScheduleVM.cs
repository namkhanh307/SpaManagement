namespace Repos.ViewModels.ScheduleVM
{
    public class PostScheduleVM
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
