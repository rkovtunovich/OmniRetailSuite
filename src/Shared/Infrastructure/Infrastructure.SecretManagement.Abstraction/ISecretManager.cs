namespace Infrastructure.SecretManagement.Abstraction;

public interface ISecretManager
{
    Task<string> GetSecretAsync(string secretName);

    Task<string> GetSecretAsync(string secretName, int version);
}
