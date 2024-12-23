namespace Repos.Entities;

public partial class UserScheduleBooking
{
    public string BookingId { get; set; } = string.Empty;
    public string UserScheduleId { get; set; } = string.Empty;
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public bool? Status { get; set; }
    public virtual Booking? Booking { get; set; }
}