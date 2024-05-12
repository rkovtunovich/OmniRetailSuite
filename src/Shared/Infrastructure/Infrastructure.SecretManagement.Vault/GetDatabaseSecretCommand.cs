using System.Net;
using VaultSharp.Core;

namespace Infrastructure.SecretManagement.Vault;

public class GetDatabaseSecretCommand(IVaultClient vaultClient) : IGetSecretCommand
{
    public async Task<VaultSecretResponse> ExecuteAsync(SecretRequest request)
    {
        try
        {
            var secret = await vaultClient.V1.Secrets.Database.GetCredentialsAsync(request.Path, mountPoint: request.Namespace);

            var data = new Dictionary<string, string>
            {
                { "username", secret?.Data?.Username ?? throw new InvalidOperationException("Db user is empty.") },
                { "password", secret?.Data?.Password ?? throw new InvalidOperationException("Db password is empty.") }
            };

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
