namespace Repos.Entities;

public partial class Schedule : BaseEntity
{
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public virtual ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
}