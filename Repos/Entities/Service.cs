namespace Repos.Entities;

public partial class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public double StartPrice { get; set; }
    public double EndPrice { get; set; }
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<PackageService> PackageServices { get; set; } = new List<PackageService>();
}