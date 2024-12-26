using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repos.Entities;

namespace Repos.DbContextFactory;

public partial class SpaManagementContext : IdentityDbContext<User, Role, Guid, UserClaims, UserRoles, UserLogins, RoleClaim, UserTokens>
{
    public SpaManagementContext()
    {
    }

    public SpaManagementContext(DbContextOptions<SpaManagementContext> options)
        : base(options) { }
    public override DbSet<User> Users => Set<User>();
    public override DbSet<Role> Roles => Set<Role>();
    public override DbSet<UserClaims> UserClaims => Set<UserClaims>();
    public override DbSet<UserRoles> UserRoles => Set<UserRoles>();
    public override DbSet<UserLogins> UserLogins => Set<UserLogins>();
    public override DbSet<RoleClaim> RoleClaims => Set<RoleClaim>();
    public override DbSet<UserTokens> UserTokens => Set<UserTokens>();
    public virtual DbSet<Booking> Bookings => Set<Booking>();
    public virtual DbSet<Image> Images => Set<Image>();
    public virtual DbSet<Order> Orders => Set<Order>();
    public virtual DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public virtual DbSet<Package> Packages => Set<Package>();
    public virtual DbSet<PackageImage> PackageImages => Set<PackageImage>();
    public virtual DbSet<PackageService> PackageServices => Set<PackageService>();
    public virtual DbSet<Product> Products => Set<Product>();
    public virtual DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public virtual DbSet<Salary> Salaries => Set<Salary>();
    public virtual DbSet<SalaryPerHour> SalaryPerHours => Set<SalaryPerHour>();
    public virtual DbSet<Schedule> Schedules => Set<Schedule>();
    public virtual DbSet<Service> Services => Set<Service>();
    public virtual DbSet<ServiceImage> ServiceImages => Set<ServiceImage>();
    public virtual DbSet<UserSchedule> UserSchedules => Set<UserSchedule>();
    public virtual DbSet<UserScheduleBooking> UserScheduleBookings => Set<UserScheduleBooking>();
    public virtual DbSet<Vote> Votes => Set<Vote>();

    private string? GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API")).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        return configuration["ConnectionStrings:DefaultConnection"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            string tableName = entityType.GetTableName() ?? "";
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Package).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.PackageId);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Service).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ServiceId);
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<PackageImage>(entity =>
        {
            entity.HasKey(f => new { f.PackageId, f.ImageId });
        });

        modelBuilder.Entity<PackageService>(entity =>
        {
            entity.HasKey(e => new { e.PackageId, e.ServiceId });

            entity.HasOne(d => d.Package)
                  .WithMany(d => d.PackageServices)
                  .HasForeignKey(e => e.PackageId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Service)
                  .WithMany(d => d.PackageServices)
                  .HasForeignKey(e => e.ServiceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });


        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(f => new { f.ProductId, f.ImageId });
        });


        modelBuilder.Entity<Salary>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(d => d.User).WithMany(p => p.Salaries)
               .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<SalaryPerHour>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.User).WithMany(p => p.SalaryPerHours)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ServiceImage>(entity =>
        {
            entity.HasKey(f => new { f.ServiceId, f.ImageId });
        });

        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.HasKey(e => e.Id);
        //});

        modelBuilder.Entity<UserSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
          

            entity.HasOne(d => d.Schedule)
                .WithMany(s => s.UserSchedules)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.User)
                .WithMany(u => u.UserSchedules)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UserSchedule_User");
        });

        //modelBuilder.Entity<UserRoles>(entity =>
        //{
        //    entity.HasKey(ur => new { ur.UserId, ur.RoleId });

        //    entity.HasOne(ur => ur.User)
        //        .WithMany(u => u.UserRoles)
        //        .HasForeignKey(ur => ur.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    entity.HasOne(ur => ur.Role)
        //        .WithMany()
        //        .HasForeignKey(ur => ur.RoleId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //});

        modelBuilder.Entity<UserScheduleBooking>(entity =>
        {
            entity.HasKey(e => new { e.UserScheduleId, e.BookingId });
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}