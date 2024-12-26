using Microsoft.AspNetCore.Identity;

namespace Repos.Entities
{
    public class UserRoles : IdentityUserRole<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool? Status { get; set; }
        //public virtual User? User { get; set; }
        //public virtual Role? Role { get; set; }
        public UserRoles()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
