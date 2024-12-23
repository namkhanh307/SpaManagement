namespace Repos.Entities;

public partial class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}