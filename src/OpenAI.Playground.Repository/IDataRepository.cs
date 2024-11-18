namespace OpenAI.Playground.Repository;

public interface IDataRepository
{
    Task<IEnumerable<dynamic>> ExecuteQuery(
        string query,
        CancellationToken cancellationToken = default
    );
    Task<string> GetDatabaseSchema(CancellationToken cancellationToken = default);
}
