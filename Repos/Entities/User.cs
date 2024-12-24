using Microsoft.AspNetCore.Identity;

namespace Repos.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool? Status { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<SalaryPerHour> SalaryPerHours { get; set; } = new List<SalaryPerHour>();
        public virtual ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
        public virtual ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
