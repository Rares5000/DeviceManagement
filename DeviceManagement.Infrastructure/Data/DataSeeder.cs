using DeviceManagement.Core.Entities;
using DeviceManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync() || await context.Devices.AnyAsync())
            return;

        var users = new List<User>
        {
            new() { Name = "User Test1", Email = "test1@company.com", Role = "Developer", Location = "Bucharest", PasswordHash = "placeholder" },
            new() { Name = "User Test2", Email = "test2@company.com", Role = "QA Engineer", Location = "Cluj", PasswordHash = "placeholder" },
            new() { Name = "User Test3", Email = "test3@company.com", Role = "Designer", Location = "Timisoara", PasswordHash = "placeholder" },
            new() { Name = "User Test4", Email = "test4@company.com", Role = "Manager", Location = "Iasi", PasswordHash = "placeholder" }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

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
                Description = "High-performance Apple smartphone.",
                SerialNumber = "APL-001",
                UserId = users[0].Id
            },
            new() {
                Name = "Samsung Galaxy S24",
                Manufacturer = "Samsung",
                Type = DeviceType.Phone,
                OperatingSystem = "Android",
                OsVersion = "14.0",
                Processor = "Snapdragon 8 Gen 3",
                RamAmount = 12,
                Description = "Flagship Samsung smartphone.",
                SerialNumber = "SAM-001",
                UserId = users[1].Id
            },
            new() {
                Name = "iPad Pro 12.9",
                Manufacturer = "Apple",
                Type = DeviceType.Tablet,
                OperatingSystem = "iPadOS",
                OsVersion = "17.0",
                Processor = "M2",
                RamAmount = 16,
                Description = "Professional Apple tablet.",
                SerialNumber = "APL-002",
                UserId = users[2].Id
            },
            new() {
                Name = "Samsung Galaxy Tab S9",
                Manufacturer = "Samsung",
                Type = DeviceType.Tablet,
                OperatingSystem = "Android",
                OsVersion = "13.0",
                Processor = "Snapdragon 8 Gen 2",
                RamAmount = 12,
                Description = "Premium Samsung tablet.",
                SerialNumber = "SAM-002",
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
                Description = "Google's flagship smartphone.",
                SerialNumber = "GOG-001",
                UserId = null
            }
        };

        await context.Devices.AddRangeAsync(devices);
        await context.SaveChangesAsync();
    }
}