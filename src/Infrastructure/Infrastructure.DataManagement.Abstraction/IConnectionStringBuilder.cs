namespace Infrastructure.DataManagement.Abstraction;
public interface IConnectionStringBuilder
{
    Task<string> BuildConnectionString();
}
