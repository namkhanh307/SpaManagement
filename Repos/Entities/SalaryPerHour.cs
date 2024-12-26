namespace Repos.Entities;

public partial class SalaryPerHour : BaseEntity
{
    public string UserId { get; set; }
    public double Amount { get; set; }
    public virtual User? User { get; set; }
}