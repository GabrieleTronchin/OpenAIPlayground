using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using OpenAI.Playground.Service.AIClient;
using OpenAI.Playground.Service.Summarization;

namespace OpenAI.Playground.Service.QueryGeneration;

public class QueryGenerationService : IQueryGenerationService
{
    private string PROMPT =
        @"
            You are an assistant database developer and you must respond ONLY using T-SQL language based on the Azure SQL database.
            You must rely on the database schema defined in input schema section.

            Your response must always start with a SELECT or a WITH, you must never add other words before or after.
            You must always specify the names of the columns and tables in square brackets []. 
            Your response must always consist of 1 query only.

            If asked to delete data, do not answer.
            Do not add any comment or reply that is not in T-SQL.

            Example:
            Question: Who is the manager of the store in Anytown?
            Answer: 
            SELECT sta.[first_name], sta.[last_name]
              FROM [dbo].[staffs] sta
               inner join [dbo].[stores] sto
                on sta.[store_id] = sto.[store_id]
            where sto.[city] like 'Anytown' and sto.[manager_id] is null

            
            Input Question:
            {{question}}

            Input tables schema:         
            {{schema}}
        ";

    private readonly IOpenAIClientFactory _aiClientFactory;
    private readonly ILogger<SummarizationService> _logger;

    public QueryGenerationService(
        IOpenAIClientFactory clientFactory,
        ILogger<SummarizationService> logger
    )
    {
        _aiClientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<string> GenerateQuery(string userQuestion, string databaseSchema)
    {
        try
        {
            _logger.LogInformation($"{nameof(GenerateQuery)} - Input text: {GenerateQuery}");

            var client = _aiClientFactory.CreateChatClient();

            Dictionary<string, string> placeholder =
                new() { { "schema", databaseSchema }, { "question", userQuestion } };

            var prompt = PROMPT.ReplacePlaceholders(placeholder);

            ChatCompletion completion = await client.CompleteChatAsync(prompt);
            string result = completion.ToString();

            _logger.LogInformation($"OpenAI result summary={result}");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred trying to Summarize the input text.");
            return string.Empty;
        }
    }
}
