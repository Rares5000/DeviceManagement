using DeviceManagement.Core.Entities;

namespace DeviceManagement.Core.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}