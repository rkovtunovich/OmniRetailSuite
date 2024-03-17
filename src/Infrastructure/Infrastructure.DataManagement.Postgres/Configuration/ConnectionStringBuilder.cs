namespace Infrastructure.DataManagement.Postgres.Configuration;

public class ConnectionStringBuilder(IOptions<DbSettings> options, ISecretManager secretManager) : IConnectionStringBuilder
{
    public async Task<string> BuildConnectionString()
    {
        var user = await secretManager.GetSecretAsync("sname")
            ?? throw new InvalidOperationException("Db user is empty.");

        var password = await secretManager.GetSecretAsync("spassword") 
            ?? throw new InvalidOperationException("Db password is empty.");

        var settings = options.Value;

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = settings.Host,
            Port = settings.Port,
            Username = user,
            Password = password,
            Database = settings.Database,
        };

        return builder.ConnectionString;
    }
}
