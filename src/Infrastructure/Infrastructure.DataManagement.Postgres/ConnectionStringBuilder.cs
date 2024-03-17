namespace Infrastructure.DataManagement.Postgres;

public class ConnectionStringBuilder(IOptions<DbSettings> options, ISecretManager secretManager) : IConnectionStringBuilder
{
    public async Task<string> BuildConnectionString()
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
