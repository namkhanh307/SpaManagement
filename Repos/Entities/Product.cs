namespace Repos.Entities;

public partial class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}