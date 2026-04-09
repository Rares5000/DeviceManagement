using DeviceManagement.Core.Interfaces;
using DeviceManagement.Core.Settings;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace DeviceManagement.Infrastructure.Services;

public class AIService : IAIService
{
    private readonly AISettings _aiSettings;

    public AIService(AISettings aiSettings)
    {
        _aiSettings = aiSettings;
    }

    public async Task<string> GenerateDeviceDescriptionAsync(
        string name,
        string manufacturer,
        string os,
        string type,
        int ram,
        string processor)
    {
        var clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri("https://api.groq.com/openai/v1")
        };

        var client = new ChatClient(
            model: _aiSettings.Model,
            credential: new ApiKeyCredential(_aiSettings.ApiKey),
            options: clientOptions
        );

        var prompt = $"""
            Generate a clear, relevant, and user-friendly description for a mobile device with these specs:
            - Name: {name}
            - Manufacturer: {manufacturer}
            - Type: {type}
            - Operating System: {os}
            - Processor: {processor}
            - RAM: {ram}GB

            Write 1-2 sentences that enhance the device information in a way that is easy to understand 
            for any user, highlighting the key strengths and practical benefits of this device.
            Return only the description, no extra text.
        """;

        var messages = new List<ChatMessage>
        {
            new UserChatMessage(prompt)
        };

        var options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = 150
        };

        var response = await client.CompleteChatAsync(messages, options);
        return response.Value.Content[0].Text ?? string.Empty;
    }
}