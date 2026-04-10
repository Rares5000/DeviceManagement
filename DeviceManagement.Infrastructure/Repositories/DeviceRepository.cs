using DeviceManagement.Core.Entities;
using DeviceManagement.Core.Interfaces;
using DeviceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly AppDbContext _context;

    public DeviceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _context.Devices
            .Include(d => d.User)
            .ToListAsync();
    }

    public async Task<Device?> GetByIdAsync(int id)
    {
        return await _context.Devices
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Device> CreateAsync(Device device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<Device> UpdateAsync(Device device)
    {
        _context.Devices.Update(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task DeleteAsync(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device != null)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> SerialNumberExistsAsync(string serialNumber)
    {
        return await _context.Devices
            .AnyAsync(d => d.SerialNumber.ToLower() == serialNumber.ToLower());
    }

    public async Task<IEnumerable<Device>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return await GetAllAsync();   
        }

        var tokens = NormalizeQuery(query)
            .Trim()
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var devices = await _context.Devices
            .Include(d => d.User)
            .ToListAsync();

        var scoredDevices = devices
            .Select(device => new
            {
                Device = device,
                Score = CalculateScore(device, tokens)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Device)
            .ToList();

        return scoredDevices;
    }

    private static int CalculateScore(Device device, string[] tokens)
    {
        var score = 0;

        foreach (var token in tokens)
        {
            if (device.Name.ToLower().Contains(token))
                score += 10;

            if (device.Manufacturer.ToLower().Contains(token))
                score += 7;

            if (device.Processor.ToLower().Contains(token))
                score += 5;

            if (device.RamAmount.ToString().Contains(token))
                score += 3;
        }

        return score;
    }

    private static string NormalizeQuery(string query)
    {
        return new string(query
            .Where(c => !char.IsPunctuation(c))
            .ToArray());
    }
}