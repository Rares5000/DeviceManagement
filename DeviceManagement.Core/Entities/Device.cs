using DeviceManagement.Core.Enums;

namespace DeviceManagement.Core.Entities;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public string OperatingSystem { get; set; } = string.Empty;
    public string OsVersion { get; set; } = string.Empty;
    public string Processor { get; set; } = string.Empty;
    public int RamAmount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty; // I added this value in case of duplication
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; }
    public User? User { get; set; }
}