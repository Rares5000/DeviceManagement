using DeviceManagement.Core.DTOs;

namespace DeviceManagement.Core.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(string id);
}