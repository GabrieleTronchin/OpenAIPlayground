using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using OpenAI.Playground.Service.Options;

namespace OpenAI.Playground.Service.AIClient;

public class OpenAIClientFactory : IOpenAIClientFactory
{
    private readonly OpenAIOptions _configuration;
    private readonly ILogger<OpenAIClientFactory> _logger;

    public OpenAIClientFactory(
        IOptions<OpenAIOptions> configuration,
        ILogger<OpenAIClientFactory> logger
    )
    {
        _configuration = configuration.Value;
        _logger = logger;
    }

    public ChatClient CreateChatClient(string modelName = "")
    {
        if (string.IsNullOrWhiteSpace(modelName))
            modelName = _configuration.DefaultModelName;

        AzureOpenAIClient azureOpenAIClient =
            new(new Uri(_configuration.Endpoint), new DefaultAzureCredential());
        return azureOpenAIClient.GetChatClient(modelName);
    }
}
