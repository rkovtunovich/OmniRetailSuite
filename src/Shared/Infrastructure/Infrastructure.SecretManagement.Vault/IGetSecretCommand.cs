namespace Infrastructure.SecretManagement.Vault;

public interface IGetSecretCommand
{
    Task<VaultSecretResponse> ExecuteAsync(SecretRequest request);
}
