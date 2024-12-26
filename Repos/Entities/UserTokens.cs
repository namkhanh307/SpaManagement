using Microsoft.AspNetCore.Identity;

namespace Repos.Entities
{
    public class UserTokens : IdentityUserToken<string>
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiredTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool? Status { get; set; }
        public UserTokens()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
