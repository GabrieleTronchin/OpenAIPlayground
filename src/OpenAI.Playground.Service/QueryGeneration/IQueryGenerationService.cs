namespace OpenAI.Playground.Service.QueryGeneration;

public interface IQueryGenerationService
{
    Task<string> GenerateQuery(string userQuestion, string databaseSchema);
}
