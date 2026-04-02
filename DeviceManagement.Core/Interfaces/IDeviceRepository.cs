using DeviceManagement.Core.Entities;

namespace DeviceManagement.Core.Interfaces;

public interface IDeviceRepository
{
    Task<IEnumerable<Device>> GetAllAsync();
    Task<Device?> GetByIdAsync(int id);
    Task<Device> CreateAsync(Device device);
    Task<Device> UpdateAsync(Device device);
    Task DeleteAsync(int id);
    Task<bool> SerialNumberExistsAsync(string serialNumber);
}