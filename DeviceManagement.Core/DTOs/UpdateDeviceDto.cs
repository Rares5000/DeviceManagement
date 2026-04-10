using System.ComponentModel.DataAnnotations;
using DeviceManagement.Core.Enums;

namespace DeviceManagement.Core.DTOs;

public class UpdateDeviceDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Manufacturer { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(DeviceType), ErrorMessage = "Type must be Phone or Tablet")]
    public DeviceType Type { get; set; }

    [Required]
    public string OperatingSystem { get; set; } = string.Empty;

    [Required]
    public string OsVersion { get; set; } = string.Empty;

    [Required]
    public string Processor { get; set; } = string.Empty;

    [Range(1, 64, ErrorMessage = "RAM must be between 1 and 64 GB")]
    public int RamAmount { get; set; }

    public string Description { get; set; } = string.Empty;

    [Required]
    public string SerialNumber { get; set; } = string.Empty;
}