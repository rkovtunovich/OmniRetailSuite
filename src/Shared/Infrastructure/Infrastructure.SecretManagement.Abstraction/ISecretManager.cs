namespace Infrastructure.SecretManagement.Abstraction;

public interface ISecretManager
{
    Task<string> GetSecretAsync(SecretRequest request);
}
