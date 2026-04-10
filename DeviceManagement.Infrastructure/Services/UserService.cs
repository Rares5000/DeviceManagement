using DeviceManagement.Core.DTOs;
using DeviceManagement.Core.Interfaces;
using Mapster;

namespace DeviceManagement.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Adapt<List<UserDto>>();
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        return user.Adapt<UserDto>();
    }
}