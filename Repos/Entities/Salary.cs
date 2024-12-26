namespace Repos.Entities;

public partial class Salary : BaseEntity
{
    public Guid UserId { get; set; }
    public double Total { get; set; }
    public virtual User? User { get; set; }

}