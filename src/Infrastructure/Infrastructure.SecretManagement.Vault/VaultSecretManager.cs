using Infrastructure.SecretManagement.Abstraction;
using VaultSharp;

namespace Infrastructure.SecretManagement.Vault;

public class VaultSecretManager : ISecretManager
{
    private readonly IVaultClient _vaultClient;

    public VaultSecretManager(IVaultClient vaultClient)
    {
        _vaultClient = vaultClient;
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(secretName);
        return secret?.Data?.Data["value"]?.ToString() ?? string.Empty;
    }

    public async Task<string> GetSecretAsync(string secretName, int version)
    {
        var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(secretName, version);
        return secret?.Data?.Data["value"]?.ToString() ?? string.Empty;
    }
}
