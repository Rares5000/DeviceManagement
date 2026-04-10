using DeviceManagement.Core.Entities;
using DeviceManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Devices.AnyAsync())
            return;

        var devices = new List<Device>
        {
            new() {
                Name = "iPhone 15 Pro",
                Manufacturer = "Apple",
                Type = DeviceType.Phone,
                OperatingSystem = "iOS",
                OsVersion = "17.0",
                Processor = "A17 Pro",
                RamAmount = 8,
                SerialNumber = "APL-001",
                Description = "High-performance Apple smartphone.",
                UserId = null
            },
            new() {
                Name = "Samsung Galaxy S24",
                Manufacturer = "Samsung",
                Type = DeviceType.Phone,
                OperatingSystem = "Android",
                OsVersion = "14.0",
                Processor = "Snapdragon 8 Gen 3",
                RamAmount = 12,
                SerialNumber = "SAM-001",
                Description = "Flagship Samsung smartphone.",
                UserId = null
            },
            new() {
                Name = "iPad Pro 12.9",
                Manufacturer = "Apple",
                Type = DeviceType.Tablet,
                OperatingSystem = "iPadOS",
                OsVersion = "17.0",
                Processor = "M2",
                RamAmount = 16,
                SerialNumber = "APL-002",
                Description = "Professional Apple tablet.",
                UserId = null
            },
            new() {
                Name = "Samsung Galaxy Tab S9",
                Manufacturer = "Samsung",
                Type = DeviceType.Tablet,
                OperatingSystem = "Android",
                OsVersion = "13.0",
                Processor = "Snapdragon 8 Gen 2",
                RamAmount = 12,
                SerialNumber = "SAM-002",
                Description = "Premium Samsung tablet.",
                UserId = null
            },
            new() {
                Name = "Pixel 8 Pro",
                Manufacturer = "Google",
                Type = DeviceType.Phone,
                OperatingSystem = "Android",
                OsVersion = "14.0",
                Processor = "Google Tensor G3",
                RamAmount = 12,
                SerialNumber = "GOG-001",
                Description = "Google flagship smartphone.",
                UserId = null
            }
        };

        await context.Devices.AddRangeAsync(devices);
        await context.SaveChangesAsync();
    }
}