namespace Infrastructure.SecretManagement.Abstraction;

public interface ISecretManager
{
    Task<Dictionary<string, string>> GetSecretAsync(SecretRequest request);
}
