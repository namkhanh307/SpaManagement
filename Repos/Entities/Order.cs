namespace Repos.Entities;

public partial class Order : BaseEntity
{
    public string UserId { get; set; } = string.Empty;  
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual User? User { get; set; }
}