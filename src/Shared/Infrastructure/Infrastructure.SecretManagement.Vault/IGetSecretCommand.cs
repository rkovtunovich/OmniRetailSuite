namespace Infrastructure.SecretManagement.Vault;

public interface IGetSecretCommand
{
    Task<Dictionary<string, string>> ExecuteAsync(SecretRequest request);
}
