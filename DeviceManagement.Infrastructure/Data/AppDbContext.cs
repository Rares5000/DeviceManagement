using DeviceManagement.Core.Entities;
using DeviceManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Device> Devices { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Manufacturer).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Type).HasConversion<string>();
            entity.Property(d => d.OperatingSystem).IsRequired().HasMaxLength(50);
            entity.Property(d => d.OsVersion).IsRequired().HasMaxLength(50);
            entity.Property(d => d.Processor).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Description).HasMaxLength(500);
            entity.Property(d => d.SerialNumber).IsRequired().HasMaxLength(100);

            entity.HasIndex(d => d.SerialNumber).IsUnique();

            entity.HasOne(d => d.User)
                  .WithMany(u => u.Devices)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Location).IsRequired().HasMaxLength(100);
        });
    }
}