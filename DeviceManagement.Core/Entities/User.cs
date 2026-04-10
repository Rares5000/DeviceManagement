using Microsoft.AspNetCore.Identity;

namespace DeviceManagement.Core.Entities;

public class User : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Device> Devices { get; set; } = [];
}