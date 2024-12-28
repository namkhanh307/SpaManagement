using Microsoft.AspNetCore.Identity;
using Repos.ViewModels;

namespace Repos.Entities
{
    public class Role : IdentityRole<string>, IHasAttribute
    {
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool? Status { get; set; }
        public Role()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

    }
}