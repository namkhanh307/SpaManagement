namespace Repos.Entities;

public partial class Salary : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public double Total { get; set; }
    public int Month {  get; set; }
    public int Year { get; set; }   
    public virtual User? User { get; set; }

}