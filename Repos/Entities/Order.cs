namespace Repos.Entities;

public partial class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual User? User { get; set; }
}