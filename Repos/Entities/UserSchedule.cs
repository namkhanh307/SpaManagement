namespace Repos.Entities;

public partial class UserSchedule : BaseEntity
{
    public Guid UserId { get; set; }
    public string ScheduleId { get; set; } = string.Empty;
    public string SalaryId { get; set; } = string.Empty;
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public virtual Salary? Salary { get; set; }
    public virtual Schedule? Schedule { get; set; }
    public virtual User? User { get; set; }
}