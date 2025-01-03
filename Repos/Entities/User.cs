using Microsoft.AspNetCore.Identity;
using Repos.ViewModels;

namespace Repos.Entities
{
    public class User : IdentityUser<string>, IHasAttribute
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
        public virtual ICollection<PayRate> SalaryPerHours { get; set; } = new List<PayRate>();
        public virtual ICollection<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
        public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

        public User()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
