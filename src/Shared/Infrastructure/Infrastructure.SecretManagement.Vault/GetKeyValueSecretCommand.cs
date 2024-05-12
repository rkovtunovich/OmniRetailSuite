using System.Net;
using VaultSharp.Core;

namespace Infrastructure.SecretManagement.Vault;

public class GetKeyValueSecretCommand(IVaultClient vaultClient) : IGetSecretCommand
{
    public async Task<VaultSecretResponse> ExecuteAsync(SecretRequest request)
    {
        try
        {
            var secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(request.Path, mountPoint: request.Namespace);
            var secretData = (secret?.Data?.Data) ?? throw new InvalidOperationException($"Secret {request.Path} is empty.");

            var data = new Dictionary<string, string>();
            foreach( var key in request.SecretKeys)
            {
                if (secretData.TryGetValue(key, out var value))            
                    data.Add(key, value.ToString() ?? string.Empty);             
            }

            var response = new VaultSecretResponse
            {
                Data = data,
                Renewable = secret.Renewable,
                LeaseDuration = secret.LeaseDurationSeconds,
                LeaseId = secret.LeaseId
            };

            return response;
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
