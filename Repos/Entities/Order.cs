namespace Repos.Entities;

public partial class Order : BaseEntity
{
    public virtual Guid UserId { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual User? User { get; set; }
}