namespace Repos.Entities;

public partial class Package : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<PackageService> PackageServices { get; set; } = new List<PackageService>();
}