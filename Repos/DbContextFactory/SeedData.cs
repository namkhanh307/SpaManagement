using Core.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Repos.Entities;

namespace Repos.DbContextFactory
{
    public class SeedData
    {
        private readonly SpaManagementContext _context;
        public SeedData(SpaManagementContext context)
        {
            _context = context;
        }

        public void Initialise()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    bool dbExists = _context.Database.CanConnect();
                    if (dbExists)
                    {
                        _context.Database.Migrate();
                    }
                    else
                    {
                        _context.Database.Migrate();
                        Seed();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _context.Dispose();
            }
        }
        public class CountResult
        {
            public int TableCount { get; set; }
        }
        public void Seed()
        {
            int data = 0;
            data = _context.Roles.Count();
            if (data is 0)
            {
                Role[] roles = CreateRole();
                _context.AddRange(roles);
            }

            data = _context.Users.Count();
            if (data is 0)
            {
                User[] user = CreateUser();
                _context.AddRange(user);
            }
            _context.SaveChanges();

            AssignRoleToUser("staff", "Staff");
            AssignRoleToUser("user", "User");
            AssignRoleToUser("admin", "Admin");
            AssignRoleToUser("manager", "Manager");
        }

        private static Role[] CreateRole()
        {
            Role[] roles =
              [
                  new Role
                {
                    Name = "Manager",
                    NormalizedName = "Manager",
                    FullName ="Manager",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    FullName ="Staff",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Name = "User",
                    NormalizedName = "User",
                    FullName = "User",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new Role
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    FullName = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            ];
            return roles;
        }

        private static User[] CreateUser()
        {
            User[] users =
            [
                new User
                {
                    UserName = "staff",
                    FullName = "Staff John",
                    PhoneNumber = "staff",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = HashPasswordService.HashPasswordThrice("string")
                },
                new User
                {
                    UserName = "user",
                    FullName = "User Michael",
                    PhoneNumber = "user",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = HashPasswordService.HashPasswordThrice("string")
                },
                new User
                {
                    UserName = "admin",
                    FullName = "Admin Nathan",
                    PhoneNumber = "admin",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = HashPasswordService.HashPasswordThrice("string")
                },
                new User
                {
                    UserName = "manager",
                    FullName = "Manager Nakyum",
                    PhoneNumber = "manager",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = HashPasswordService.HashPasswordThrice("string")
                }
            ];
            return users;
        }
        private void AssignRoleToUser(string username, string roleName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);

            if (user != null && role != null)
            {
                if (!_context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
                {
                    _context.UserRoles.Add(new UserRoles
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                    _context.SaveChanges();
                }
            }
        }
    }
}
