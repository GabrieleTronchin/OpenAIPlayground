using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using OpenAI.Playground.Service.AIClient;
using OpenAI.Playground.Service.SummarizeReviews.Models;

namespace OpenAI.Playground.Service.SummarizeReviews;

public class SummarizationReviewService : ISummarizationReviewService
{
    private readonly IOpenAIClientFactory _aiClientFactory;
    private readonly ILogger<SummarizationReviewService> _logger;

    const string PROMPT =
        @"
system:
Analyze input JSON and create a single review that summarizes all the input in terms of points and descriptions.
You must consider all reviews, even if they are in different languages, and provide a summary that includes all.
In the output JSON, the field ""description"" is a dictionary where keys are languages and values are the summarized reviews from the input review descriptions.

Only provide an RFC8259-compliant JSON response following this format without deviation.
Do not provide incorrect answers.

# Input Examples

{
  ""reviews"": [
    {
      ""description"": ""Bastante cómoda, algo gruesa la tela. se ajustó perfecto a mi cuerpo. Espero que no se despinte rápido. Mido 1,79, peso 76 kilos, talla 32 de pantalón y compré la talla Grande."",
      ""score"": 5
    },
    {
      ""description"": ""丈が短めです。そして２回目の洗濯で裾が解れてきました。縫製に問題があるように思います。"",
      ""score"": 2
    },
    {
      ""description"": ""Misura un po’ grande xxl per me ma buono il modello"",
      ""score"": 4
    }
  ],
  ""outputLanguages"": [
    ""en"",
    ""it""
  ],
  ""maxNumberOfWords"": 50
}

# Desired Output

Only provide a RFC8259 compliant JSON response following this format without deviation.

Use this format to frame your answers: 
Key on Description field must be the same of input field outputLanguages.

{
  ""AverageScore"": 3.67,
  ""Description"": {
   ""it"": ""esempio di descrizione"",
   ""en"": ""sample description""
 }
}

You must only use this format, you are not allowed to change the format.


Languages to translate: {{langauges}}
Description must be maximum number of word: {{maxword}}
Only provide the output JSON as a simple text and do not user markdown.

# Input to use

{{input}}
";

    public SummarizationReviewService(
        IOpenAIClientFactory clientFactory,
        ILogger<SummarizationReviewService> logger
    )
    {
        _aiClientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<SummarizationReviewResponse> Summarize(SummarizationReviewRequest request)
    {
        try
        {
            _logger.LogInformation($"{nameof(SummarizationReviewRequest)} - Input text: {request}");

            var client = _aiClientFactory.CreateChatClient();

            Dictionary<string, string> placeholder =
                new()
                {
                    { "langauges", string.Join(",", request.OutputLanguages) },
                    { "maxword", request.MaxNumberOfWord.ToString() },
                    { "input", JsonSerializer.Serialize(request.Reviews) },
                };

            string strChat = PROMPT.ReplacePlaceholders(placeholder);

            ChatCompletion completion = await client.CompleteChatAsync(strChat);

            dynamic result =
                JsonSerializer.Deserialize<dynamic>(completion.ToString())
                ?? throw new InvalidOperationException("Invalid GPT output.");
            ;

            _logger.LogInformation($"OpenAI result summary={result}");

            return new SummarizationReviewResponse() { Success = true, Result = result };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred trying to Summarize the input text.");
            return new SummarizationReviewResponse() { Success = false, Result = ex };
        }
    }
}
