using DeviceManagement.Core.DTOs;

namespace DeviceManagement.Core.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<DeviceDto>> GetAllAsync();
    Task<DeviceDto> GetByIdAsync(int id);
    Task<DeviceDto> CreateAsync(CreateDeviceDto deviceDto);
    Task<DeviceDto> UpdateAsync(int id, UpdateDeviceDto deviceDto);
    Task DeleteAsync(int id);
    Task<DeviceDto> AssignAsync(int deviceId, string userId);
    Task<DeviceDto> UnassignAsync(int deviceId, string userId);
    Task<DeviceDto> GenerateDescriptionAsync(int id);
    Task<IEnumerable<DeviceDto>> SearchAsync(string query);
}