namespace Infrastructure.DataManagement.Postgres;

public class DbManager(ISecretManager secretManager, IOptions<DbSettings> options, ILogger<DbManager> logger) : IDbManager<DbSettings>
{
    public async Task EnsureDatabaseExists(DbSettings? dbSettings = null)
    {
        var secretRequest = new SecretRequest
        {
            Namespace = "kv",
            Path = "postgres-root",
            SecretKeys = ["sname", "spassword"],
        };

        var secrets = await secretManager.GetSecretAsync(secretRequest);

        var settings = options.Value;

        // Try connecting to the target database
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = settings.Host,
            Port = settings.Port,
            Username = secrets["sname"],
            Password = secrets["spassword"],
            Database = settings.Database,
        };

        if(await IsDatabaseExists(settings.Database, connectionStringBuilder.ConnectionString))
            return;

        // Connect to the default 'postgres' database to create the target database
        connectionStringBuilder.Database = "postgres";
        await CreateDatabase(settings.Database, connectionStringBuilder.ConnectionString);      
    }

    private async Task CreateDatabase(string dbName, string connectionString)
    {
        using var connection = new NpgsqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();

            using var command = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", connection);
            await command.ExecuteNonQueryAsync();

            logger.LogInformation($"Database '{dbName}' created.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error creating database '{dbName}'.");
            throw;
        }
    }

    private async Task<bool> IsDatabaseExists(string dbName, string connectionString)
    {
        using var connection = new NpgsqlConnection(connectionString);
        try
        {
            await connection.OpenAsync();
            // If successful, the database exists, nothing more to do
            logger.LogInformation($"Database '{dbName}' already exists.");
            return true;
        }
        catch (PostgresException ex) when (ex.SqlState is "3D000")
        {
            // Database does not exist (error code 3D000)
            logger.LogInformation($"Database '{dbName}' does not exist.");
        }

        return false;
    }
}
