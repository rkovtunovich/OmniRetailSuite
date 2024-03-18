namespace Infrastructure.DataManagement.Postgres;

public class DbManager(ISecretManager secretManager, IOptions<DbSettings> options, ILogger<DbManager> logger) : IDbManager
{
    public async Task EnsureDatabaseExists(string dbName)
    {
        var secretRequest = new SecretRequest
        {
            Namespace = "kv",
            Path = "postgres-root",
            SecretName = "sname"
        };
        var user = await secretManager.GetSecretAsync(secretRequest)
            ?? throw new InvalidOperationException("Db user is empty.");

        secretRequest.SecretName = "spassword";
        var password = await secretManager.GetSecretAsync(secretRequest)
            ?? throw new InvalidOperationException("Db password is empty.");

        var settings = options.Value;

        // Try connecting to the target database
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = settings.Host,
            Port = settings.Port,
            Username = user,
            Password = password,
            Database = dbName,
        };

        if(await IsDatabaseExists(dbName, connectionStringBuilder.ConnectionString))
            return;

        // Connect to the default 'postgres' database to create the target database
        connectionStringBuilder.Database = "postgres";
        await CreateDatabase(dbName, connectionStringBuilder.ConnectionString);      
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
