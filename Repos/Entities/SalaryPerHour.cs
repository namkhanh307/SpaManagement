namespace Repos.Entities;

public partial class SalaryPerHour : BaseEntity
{
    public virtual Guid UserId { get; set; }
    public double Amount { get; set; }
    public virtual User? User { get; set; }
}