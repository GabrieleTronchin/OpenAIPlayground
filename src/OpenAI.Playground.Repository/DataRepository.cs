using System.Data.SqlClient;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OpenAI.Playground.Repository;

public class DataRepository : IDataRepository
{
    private readonly string _connectionString;
    private readonly ILogger<DataRepository> _logger;

    public DataRepository(ILogger<DataRepository> logger, IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString(nameof(DataRepository))
            ?? throw new ArgumentNullException(nameof(DataRepository));
        _logger = logger;
    }

    public async Task<string> GetDatabaseSchema(CancellationToken cancellationToken = default)
    {
        try
        {
            var query =
                @"
        SELECT
        s.name as schema_name,      
        t.name as table_name,
        (
            SELECT
                c.name as column_name,
                TYPE_NAME(c.system_type_id) as data_type,   
                cd.value as MS_Description
            FROM sys.columns AS c
            LEFT OUTER JOIN sys.extended_properties as cd 
                ON cd.major_id = c.object_id
                AND cd.minor_id = c.column_id
                AND cd.name = 'MS_Description'
            WHERE c.object_id = t.object_id
            FOR JSON PATH
        ) as columns
    FROM sys.tables AS t
    INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id";
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var result = await sqlConnection.QueryAsync(query);
            return JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred at {nameof(ExecuteQuery)}");
            throw;
        }
    }

    public async Task<IEnumerable<dynamic>> ExecuteQuery(
        string query,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            var result = await sqlConnection.QueryAsync(query);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred at {nameof(ExecuteQuery)}");
            throw;
        }
    }
}
