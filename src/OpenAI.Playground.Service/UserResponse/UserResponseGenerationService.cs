using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using OpenAI.Playground.Service.AIClient;
using OpenAI.Playground.Service.UserResponse.Models;

namespace OpenAI.Playground.Service.UserResponse;

public class UserResponseGenerationService : IUserResponseGenerationService
{
    private readonly IOpenAIClientFactory _aiClientFactory;
    private readonly ILogger<UserResponseGenerationService> _logger;

    public UserResponseGenerationService(
        IOpenAIClientFactory clientFactory,
        ILogger<UserResponseGenerationService> logger
    )
    {
        _aiClientFactory = clientFactory;
        _logger = logger;
    }

    const string PROMPT =
        @"
**System:**

You need to answare to user question using a json of datas recevice as input.
In case you are not able tp answare to the question just say it to the user.
You can ask to have more dettail or make suggestions.

Print the output as simple text without any markdown.


User Question:  
{{question}}

Data:  
{{input}}

---
";

    public async Task<string> GenerateUserResponse(GenerateResponseRequest request)
    {
        try
        {
            _logger.LogInformation($"{nameof(GenerateUserResponse)} - Input text: {request}");

            var client = _aiClientFactory.CreateChatClient();

            Dictionary<string, string> placeholder =
                new()
                {
                    { "question", string.Join(",", request.UserQuestion) },
                    { "input", JsonSerializer.Serialize(request.Datas) },
                };

            var prompt = PROMPT.ReplacePlaceholders(placeholder);

            ChatCompletion completion = await client.CompleteChatAsync(prompt);
            string result = completion.ToString();

            _logger.LogInformation($"OpenAI result summary={result}");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred trying to Summarize the input text.");
            return "Unknown";
        }
    }
}
