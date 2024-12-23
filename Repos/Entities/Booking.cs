namespace Repos.Entities;

public partial class Booking : BaseEntity
{
    public string OrderDetailId { get; set; } = string.Empty;
    public virtual OrderDetail? OrderDetail { get; set; }
}