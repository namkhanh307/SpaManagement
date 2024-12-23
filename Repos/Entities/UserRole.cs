namespace Repos.Entities;

public partial class UserRole
{
    public string RoleId { get; set; }

    public string UserId { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool Status { get; set; }

    public virtual Role Role { get; set; }

    public virtual User User { get; set; }
}