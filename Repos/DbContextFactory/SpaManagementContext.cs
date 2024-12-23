using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repos.Entities;

namespace Repos.DbContextFactory;

public partial class SpaManagementContext : DbContext
{
    public SpaManagementContext()
    {
    }

    public SpaManagementContext(DbContextOptions<SpaManagementContext> options)
        : base(options) { }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageImage> PackageImages { get; set; }

    public virtual DbSet<PackageService> PackageServices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<SalaryPerHour> SalaryPerHours { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceImage> ServiceImages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserSchedule> UserSchedules { get; set; }

    public virtual DbSet<UserScheduleBooking> UserScheduleBookings { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    public static string? GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string? connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.HasKey(f => new { f.PackageId, f.ServiceId });

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
            entity.HasOne(d => d.Package).WithMany(p => p.Services)
                .HasForeignKey(d => d.PackageId);
        });

        modelBuilder.Entity<ServiceImage>(entity =>
        {
            entity.HasKey(f => new { f.ServiceId, f.ImageId });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.UserId });

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<UserSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Salary).WithMany()
                .HasForeignKey(d => d.SalaryId);

            entity.HasOne(d => d.Schedule).WithMany()
                .HasForeignKey(d => d.ScheduleId);

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId);
        });

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