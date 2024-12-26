﻿namespace Repos.Entities;

public partial class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual User? User { get; set; }
}