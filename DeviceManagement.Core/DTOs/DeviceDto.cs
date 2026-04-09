using DeviceManagement.Core.Enums;

namespace DeviceManagement.Core.DTOs;

public class DeviceDto
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
    public string SerialNumber { get; set; } = string.Empty;
    public string? AssignedUserId { get; set; }
    public string? AssignedUserName { get; set; }
    public string? AssignedUserLocation { get; set; }
}