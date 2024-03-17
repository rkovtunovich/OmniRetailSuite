using System.Net;
using VaultSharp.Core;

namespace Infrastructure.SecretManagement.Vault;

public class VaultSecretManager : ISecretManager
{
    private readonly IVaultClient _vaultClient;

    public VaultSecretManager(IVaultClient vaultClient)
    {
        _vaultClient = vaultClient;
    }

    public async Task<string> GetSecretAsync(SecretRequest request)
    {
        try
        {
            var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(request.Path, mountPoint: request.Namespace);

            return secret?.Data?.Data?.Where(x => x.Key == request.SecretName).Select(x => x.Value.ToString()).FirstOrDefault() 
                ?? throw new InvalidOperationException($"Secret {request.Path} is empty.");
        }
        catch (VaultApiException ex) when (ex.StatusCode is (int)HttpStatusCode.NotFound)
        {
            throw new InvalidOperationException($"Secret {request.Path}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error reading secret {request.Path}", ex);
        }
    }
}
