using System.Net;
using System.Net.Http.Json;
using DeviceManagement.Core.DTOs;
using DeviceManagement.Core.Enums;
using DeviceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DeviceManagement.Infrastructure.Repositories;
using DeviceManagement.Infrastructure.Services;
using DeviceManagement.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using DeviceManagement.Tests.Helpers;

namespace DeviceManagement.Tests.Integration;

public class DevicesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DevicesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptors = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                    d.ServiceType == typeof(AppDbContext) ||
                    d.ServiceType.ToString().Contains("SqlServer") ||
                    d.ServiceType.ToString().Contains("EntityFrameworkCore"))
                    .ToList();

                foreach (var descriptor in descriptors)
                    services.Remove(descriptor);

                var dbName = Guid.NewGuid().ToString();
                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase(dbName));

                services.AddScoped<IDeviceRepository, DeviceRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IDeviceService, DeviceService>();
                services.AddScoped<IUserService, UserService>();

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions,
                    TestAuthHandler>("Test", options => { });
            });
        });
    }

    private HttpClient CreateClient() => _factory.CreateClient();

    private static CreateDeviceDto BuildCreateDeviceDto(string serialNumber = "TEST-001") => new()
    {
        Name = "iPhone 15 Pro",
        Manufacturer = "Apple",
        Type = DeviceType.Phone,
        OperatingSystem = "iOS",
        OsVersion = "17.0",
        Processor = "A17 Pro",
        RamAmount = 8,
        SerialNumber = serialNumber,
        Description = "Test device"
    };

    #region GET ALL

    [Fact]
    public async Task GetAll_EmptyDatabase_ReturnsOkWithEmptyList()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/devices");
        var devices = await response.Content.ReadJsonAsync<List<DeviceDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(devices);
        Assert.Empty(devices);
    }

    [Fact]
    public async Task GetAll_WithDevices_ReturnsAllDevices()
    {
        var client = CreateClient();

        await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto("SN-001"));
        await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto("SN-002"));

        var response = await client.GetAsync("/api/devices");
        var devices = await response.Content.ReadJsonAsync<List<DeviceDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, devices!.Count);
    }

    #endregion

    #region GET BY ID

    [Fact]
    public async Task GetById_ExistingDevice_ReturnsDevice()
    {
        var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto());
        var created = await createResponse.Content.ReadJsonAsync<DeviceDto>();

        var response = await client.GetAsync($"/api/devices/{created!.Id}");
        var device = await response.Content.ReadJsonAsync<DeviceDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(created.Id, device!.Id);
        Assert.Equal(created.Name, device.Name);
        Assert.Equal(created.SerialNumber, device.SerialNumber);
    }

    [Fact]
    public async Task GetById_NonExistingDevice_ReturnsNotFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/devices/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region CREATE

    [Fact]
    public async Task Create_ValidDevice_ReturnsCreatedWithCorrectData()
    {
        var client = CreateClient();
        var dto = BuildCreateDeviceDto();

        var response = await client.PostAsJsonAsync("/api/devices", dto);
        var created = await response.Content.ReadJsonAsync<DeviceDto>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(dto.Name, created.Name);
        Assert.Equal(dto.Manufacturer, created.Manufacturer);
        Assert.Equal(dto.SerialNumber, created.SerialNumber);
        Assert.Equal(dto.RamAmount, created.RamAmount);
        Assert.Null(created.AssignedUserName);
    }

    [Fact]
    public async Task Create_DuplicateSerialNumber_ReturnsConflict()
    {
        var client = CreateClient();

        await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto("DUPLICATE-SN"));

        var duplicate = BuildCreateDeviceDto("DUPLICATE-SN");
        duplicate.Name = "Alt telefon";
        var response = await client.PostAsJsonAsync("/api/devices", duplicate);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Create_DeviceIsAccessibleAfterCreation()
    {
        var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto());
        var created = await createResponse.Content.ReadJsonAsync<DeviceDto>();

        var getResponse = await client.GetAsync($"/api/devices/{created!.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    #endregion

    #region UPDATE

    [Fact]
    public async Task Update_ExistingDevice_ReturnsUpdatedDevice()
    {
        var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto());
        var created = await createResponse.Content.ReadJsonAsync<DeviceDto>();

        var updateDto = new UpdateDeviceDto
        {
            Name = "iPhone 15 Pro Max",
            Manufacturer = "Apple",
            Type = DeviceType.Phone,
            OperatingSystem = "iOS",
            OsVersion = "17.1",
            Processor = "A17 Pro",
            RamAmount = 12,
            SerialNumber = "TEST-001",
            Description = "Updated description"
        };

        var response = await client.PutAsJsonAsync($"/api/devices/{created!.Id}", updateDto);
        var updated = await response.Content.ReadJsonAsync<DeviceDto>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("iPhone 15 Pro Max", updated!.Name);
        Assert.Equal(12, updated.RamAmount);
        Assert.Equal("17.1", updated.OsVersion);
        Assert.Equal("Updated description", updated.Description);
    }

    [Fact]
    public async Task Update_NonExistingDevice_ReturnsNotFound()
    {
        var client = CreateClient();

        var updateDto = new UpdateDeviceDto
        {
            Name = "Test",
            Manufacturer = "Test",
            Type = DeviceType.Phone,
            OperatingSystem = "Android",
            OsVersion = "14.0",
            Processor = "Test",
            RamAmount = 8,
            SerialNumber = "NONEXISTENT-SN"
        };

        var response = await client.PutAsJsonAsync("/api/devices/9999", updateDto);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithDuplicateSerialNumber_ReturnsConflict()
    {
        var client = CreateClient();
        await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto("FIRST-SN"));

        var secondResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto("SECOND-SN"));
        var second = await secondResponse.Content.ReadJsonAsync<DeviceDto>();

        var updateDto = new UpdateDeviceDto
        {
            Name = "Test",
            Manufacturer = "Apple",
            Type = DeviceType.Phone,
            OperatingSystem = "iOS",
            OsVersion = "17.0",
            Processor = "A17",
            RamAmount = 8,
            SerialNumber = "FIRST-SN" 
        };

        var response = await client.PutAsJsonAsync($"/api/devices/{second!.Id}", updateDto);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithSameSerialNumber_ReturnsOk()
    {
        var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto("MY-SN"));
        var created = await createResponse.Content.ReadJsonAsync<DeviceDto>();

        var updateDto = new UpdateDeviceDto
        {
            Name = "Updated Name",
            Manufacturer = "Apple",
            Type = DeviceType.Phone,
            OperatingSystem = "iOS",
            OsVersion = "17.0",
            Processor = "A17 Pro",
            RamAmount = 8,
            SerialNumber = "MY-SN"
        };

        var response = await client.PutAsJsonAsync($"/api/devices/{created!.Id}", updateDto);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region DELETE

    [Fact]
    public async Task Delete_ExistingDevice_ReturnsNoContent()
    {
        var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto());
        var created = await createResponse.Content.ReadJsonAsync<DeviceDto>();

        var response = await client.DeleteAsync($"/api/devices/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingDevice_IsNoLongerAccessible()
    {
        var client = CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/devices", BuildCreateDeviceDto());
        var created = await createResponse.Content.ReadJsonAsync<DeviceDto>();

        await client.DeleteAsync($"/api/devices/{created!.Id}");

        var getResponse = await client.GetAsync($"/api/devices/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_NonExistingDevice_ReturnsNotFound()
    {
        var client = CreateClient();

        var response = await client.DeleteAsync("/api/devices/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion
}