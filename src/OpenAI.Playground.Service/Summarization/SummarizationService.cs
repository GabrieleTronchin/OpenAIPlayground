using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using OpenAI.Playground.Service.AIClient;
using OpenAI.Playground.Service.Summarization.Models;

namespace OpenAI.Playground.Service.Summarization;

public class SummarizationService : ISummarizationService
{
    private readonly IOpenAIClientFactory _aiClientFactory;
    private readonly ILogger<SummarizationService> _logger;

    public SummarizationService(
        IOpenAIClientFactory clientFactory,
        ILogger<SummarizationService> logger
    )
    {
        _aiClientFactory = clientFactory;
        _logger = logger;
    }

    const string PROMPT =
        @"
system:

You will receive an input text, some output languages and the maximum word allowed for the output summarized text.
You need to summarize an input text, that could be in any language, and translate the summarized text in any language required.
You must be more coherent with the input text as possible in terms of meaning, but you allow to reinterpret some concept if the result have the same meaning.
You not allow to change code, ID, or other references.

You must print the output as a plain text and not use markdown syntax.

# Input Output Sample

{
  ""textToSummarize"": ""The input text to be summarized"",
  ""targetLanguages"": [
    ""en"",
    ""it""
  ],
  ""maxNumberOfWords"": 50
}


# Desired Output Sample

Only provide a RFC8259 compliant JSON response following this format without deviation.

Use this format to frame your answers: 
Key on Description field must be the same of input field outputLanguages.

{
  ""SummarizedTexts"": {
   ""it"": ""Summarized text in Italian"",
   ""en"": ""Summarized text in English""
 }
}

# Real Input to use

{{input}}
";

    public async Task<SummarizationResponse> TextSummarize(SummarizationRequest request)
    {
        try
        {
            _logger.LogInformation($"{nameof(TextSummarize)} - Input text: {request}");
            var client = _aiClientFactory.CreateChatClient();

            Dictionary<string, string> placeholder =
                new() { { "input", JsonSerializer.Serialize(request) } };

            string strChat = PROMPT.ReplacePlaceholders(placeholder);

            ChatCompletion completion = await client.CompleteChatAsync(strChat);

            dynamic result =
                JsonSerializer.Deserialize<dynamic>(completion.ToString())
                ?? throw new InvalidOperationException("Invalid GPT output.");
            ;

            _logger.LogInformation($"OpenAI result summary={result}");

            return new SummarizationResponse() { Success = true, Result = result };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred trying to Summarize the input text.");
            return new SummarizationResponse() { Success = false, Result = ex };
        }
    }
}
