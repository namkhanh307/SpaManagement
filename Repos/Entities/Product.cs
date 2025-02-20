﻿namespace Repos.Entities;

public partial class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}