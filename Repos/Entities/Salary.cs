namespace Repos.Entities;

public partial class Salary : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public double Total { get; set; }
    public virtual User? User { get; set; }

}