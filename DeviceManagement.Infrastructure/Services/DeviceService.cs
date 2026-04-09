using DeviceManagement.Core.DTOs;
using DeviceManagement.Core.Entities;
using DeviceManagement.Core.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DeviceManagement.Infrastructure.Services;

public class DeviceService : IDeviceService
{
    public readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;

    public DeviceService(IDeviceRepository deviceRepository, IUserRepository userRepository)
    {
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<DeviceDto>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        return devices.Adapt<List<DeviceDto>>();
    }

    public async Task<DeviceDto> GetByIdAsync(int id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if(device == null)
        {
            throw new KeyNotFoundException("Device not found");
        }
        
        return device.Adapt<DeviceDto>();
    }

    public async Task<DeviceDto> CreateAsync(CreateDeviceDto deviceDto)
    {
        var serialExists = await _deviceRepository.SerialNumberExistsAsync(deviceDto.SerialNumber);
        if(serialExists)
        {
            throw new InvalidOperationException("A device with this serial number already exists");
        }

        var device = deviceDto.Adapt<Device>();
        var created = await _deviceRepository.CreateAsync(device);

        return created.Adapt<DeviceDto>();
    }

    public async Task<DeviceDto> UpdateAsync(int id, UpdateDeviceDto deviceDto)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if(device == null)
        {
            throw new KeyNotFoundException("Device not found.");
        }

        if (device.SerialNumber != deviceDto.SerialNumber)
        {
            var serialExists = await _deviceRepository.SerialNumberExistsAsync(deviceDto.SerialNumber);
            if (serialExists)
            {
                throw new InvalidOperationException("A device with this serial number already exists.");
            }
        }

        deviceDto.Adapt(device);

        var updated = await _deviceRepository.UpdateAsync(device);
        return updated.Adapt<DeviceDto>();
    }

    public async Task DeleteAsync(int id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if(device == null)
        {
            throw new KeyNotFoundException("Device not found.");
        }

        await _deviceRepository.DeleteAsync(id);
    }

    public async Task<DeviceDto> AssignAsync(int deviceId, string userId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device == null)
            throw new KeyNotFoundException("Device not found.");

        if (device.UserId != null)
            throw new InvalidOperationException("Device is already assigned to another user.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        device.UserId = userId;
        var updated = await _deviceRepository.UpdateAsync(device);
        return updated.Adapt<DeviceDto>();
    }

    public async Task<DeviceDto> UnassignAsync(int deviceId, string userId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        if (device == null)
            throw new KeyNotFoundException("Device not found.");

        if (device.UserId != userId)
            throw new InvalidOperationException("This device is not assigned to you.");

        device.UserId = null;
        var updated = await _deviceRepository.UpdateAsync(device);
        return updated.Adapt<DeviceDto>();
    }
}