namespace Infrastructure.DataManagement.Abstraction;

public interface IDbManager<TOptions> where TOptions : class
{
    Task EnsureDatabaseExists(TOptions? options = null);
}
