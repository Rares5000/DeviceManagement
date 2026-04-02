using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeviceManagement.Tests.Helpers;

public static class HttpContentExtensions
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task<T?> ReadJsonAsync<T>(this HttpContent content)
    {
        return await content.ReadFromJsonAsync<T>(Options);
    }
}