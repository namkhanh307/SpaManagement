namespace Repos.Entities;

public partial class User : BaseEntity
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public int? Gender { get; set; }

    public string Avatar { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<SalaryPerHour> SalaryPerHours { get; set; } = new List<SalaryPerHour>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}