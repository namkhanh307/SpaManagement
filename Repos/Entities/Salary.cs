namespace Repos.Entities;

public partial class Salary : BaseEntity
{
    public string UserId { get; set; }
    public double Total { get; set; }
    public virtual User? User { get; set; }

}