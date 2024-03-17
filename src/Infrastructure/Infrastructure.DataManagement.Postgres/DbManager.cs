namespace Infrastructure.DataManagement.Postgres;

public class DbManager(IConnectionStringBuilder connectionStringBuilder, ILogger<DbManager> logger) : IDbManager
{
    public async Task EnsureDatabaseExists(string dbName)
    {
        var connectionString = await connectionStringBuilder.BuildConnectionString();

        // Try connecting to the target database
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = dbName,
        };

        using (var connection = new NpgsqlConnection(builder.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                // If successful, the database exists, nothing more to do
                logger.LogInformation($"Database '{dbName}' already exists.");
                return;
            }
            catch (PostgresException ex) when (ex.SqlState is "3D000")
            {
                // Database does not exist (error code 3D000)
                logger.LogInformation($"Database '{dbName}' does not exist.");
            }
        }

        // Connect to the default 'postgres' database to create the target database
        builder.Database = "postgres";
        using (var connection = new NpgsqlConnection(builder.ConnectionString))
        {
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
    }
}
