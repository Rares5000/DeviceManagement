namespace DeviceManagement.Core.Interfaces;

public interface IAIService
{
    Task<string> GenerateDeviceDescriptionAsync(string name, string manufacturer,
        string os, string type, int ram, string processor);
}