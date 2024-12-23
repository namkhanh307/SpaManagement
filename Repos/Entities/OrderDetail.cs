namespace Repos.Entities;

public partial class OrderDetail : BaseEntity
{
    public string? PackageId { get; set; }

    public string? ServiceId { get; set; }

    public string? ProductId { get; set; }

    public string? OrderId { get; set; }

    public int Quantity { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Order? Order { get; set; }

    public virtual Package? Package { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Service? Service { get; set; }
}