using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using OpenAI.Playground.Service.AIClient;
using OpenAI.Playground.Service.SmartFactory.Models;

namespace OpenAI.Playground.Service.SmartFactory;

public class AIFactory(IOpenAIClientFactory clientFactory, ILogger<AIFactory> logger) : IAIFactory
{
    private readonly IOpenAIClientFactory _aiClientFactory = clientFactory;
    private readonly ILogger<AIFactory> _logger = logger;
    const string PROMPT =
        @"
**System:**

Analyze the input JSON (a dictionary string where the key is a class name and the value is a description) along with the user question to determine which key to return to the user.

Only provide a single key as the result. If you are unable to determine an exact key, just return ""Unknown."" 

Provide the output as simple text and do not use markdown. 

You must strictly follow these rules and are not allowed to change the format.

**Input Examples:**

**Example 01:**

Input

User question:  
Which came first, the chicken or the egg?

JSON:  
{  
    ""ClassOne"": ""The question prompted by the user is about animals or flowers."",  
    ""ClassTwo"": ""The question prompted by the user is about cars.""  
}


Output:
ClassOne

---

**Example 02:**

Input

User question:  
What do you check before starting the engine?

JSON:  
{  
    ""ClassOne"": ""The question prompted by the user is about animals or flowers."",  
    ""ClassTwo"": ""The question prompted by the user is about cars.""  
}


Output:
ClassTwo

---

**Example 03:**

Input

User question:  
Is there a smoke alarm and carbon monoxide detector?

JSON:  
{  
    ""ClassOne"": ""The question prompted by the user is about animals or flowers."",  
    ""ClassTwo"": ""The question prompted by the user is about cars.""  
}


Output:
Unknown

---

**Real Input:**

User Question:  
{{question}}

Dictionary:  
{{input}}

---
";

    public async Task<FactoryResponse> Detect(FactoryRequest request)
    {
        try
        {
            _logger.LogInformation($"{nameof(Detect)} - Input text: {request}");

            var client = _aiClientFactory.CreateChatClient();

            Dictionary<string, string> placeholder =
                new()
                {
                    { "question", string.Join(",", request.UserQuestion) },
                    { "input", JsonSerializer.Serialize(request.FactoryInputs) },
                };

            var prompt = PROMPT.ReplacePlaceholders(placeholder);

            ChatCompletion completion = await client.CompleteChatAsync(prompt);
            string result = completion.ToString();

            _logger.LogInformation($"OpenAI result summary={result}");

            return new FactoryResponse() { Result = completion.ToString() };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred trying to Summarize the input text.");
            return new FactoryResponse() { Result = "Unknown" };
        }
    }
}
