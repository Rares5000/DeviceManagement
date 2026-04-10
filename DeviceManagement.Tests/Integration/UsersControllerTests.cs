using System.Net;
using System.Net.Http.Json;
using DeviceManagement.Core.DTOs;
using DeviceManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using DeviceManagement.Infrastructure.Repositories;
using DeviceManagement.Infrastructure.Services;
using DeviceManagement.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using DeviceManagement.Tests.Helpers;

namespace DeviceManagement.Tests.Integration;

public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UsersControllerTests(WebApplicationFactory<Program> factory)
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

    [Fact]
    public async Task GetAll_EmptyDatabase_ReturnsOkWithEmptyList()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/users");
        var users = await response.Content.ReadJsonAsync<List<UserDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(users);
        Assert.Empty(users);
    }

    [Fact]
    public async Task GetById_NonExistingUser_ReturnsNotFound()
    {
        var client = CreateClient();

        var response = await client.GetAsync("/api/users/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}