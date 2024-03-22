using System.Net;
using VaultSharp.Core;

namespace Infrastructure.SecretManagement.Vault;
public class GetKeyValueSecretCommand : IGetSecretCommand
{
    private readonly IVaultClient _vaultClient;

    public GetKeyValueSecretCommand(IVaultClient vaultClient)
    {
        _vaultClient = vaultClient;
    }

    public async Task<Dictionary<string, string>> ExecuteAsync(SecretRequest request)
    {
        try
        {
            var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(request.Path, mountPoint: request.Namespace);
            var secretData = (secret?.Data?.Data) ?? throw new InvalidOperationException($"Secret {request.Path} is empty.");

            var result = new Dictionary<string, string>();
            foreach( var key in request.SecretKeys)
            {
                if (secretData.TryGetValue(key, out var value))            
                    result.Add(key, value.ToString() ?? string.Empty);             
            }

            return result;
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
