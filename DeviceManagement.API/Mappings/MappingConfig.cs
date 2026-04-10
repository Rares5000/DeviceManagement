using DeviceManagement.Core.DTOs;
using DeviceManagement.Core.Entities;
using Mapster;

namespace DeviceManagement.API.Mappings;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Device, DeviceDto>.NewConfig()
            .Map(dest => dest.AssignedUserId, src => src.UserId)
            .Map(dest => dest.AssignedUserName, src => src.User != null ? src.User.Name : null)
            .Map(dest => dest.AssignedUserLocation, src => src.User != null ? src.User.Location : null);

        TypeAdapterConfig<User, UserDto>.NewConfig();

        TypeAdapterConfig<User, AuthResponseDto>.NewConfig()
            .Map(dest => dest.UserId, src => src.Id);
    }
}