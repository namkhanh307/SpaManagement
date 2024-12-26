namespace Repos.Entities;

public partial class SalaryPerHour : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public double Amount { get; set; }
    public virtual User? User { get; set; }
}