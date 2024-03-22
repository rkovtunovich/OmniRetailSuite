namespace Infrastructure.DataManagement.Abstraction;

public interface IDbManager
{
    Task EnsureDatabaseExists(string dbName);
}
