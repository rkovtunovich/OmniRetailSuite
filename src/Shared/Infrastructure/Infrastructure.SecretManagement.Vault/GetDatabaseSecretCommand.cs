using System.Net;
using Microsoft.Extensions.Logging;
using VaultSharp.Core;

namespace Infrastructure.SecretManagement.Vault;

public class GetDatabaseSecretCommand(IVaultClient vaultClient, ILogger<VaultSecretManager> logger) : IGetSecretCommand
{
    public async Task<VaultSecretResponse> ExecuteAsync(SecretRequest request)
    {
        try
        {
            var secret = await vaultClient.V1.Secrets.Database.GetCredentialsAsync(request.Path, mountPoint: request.Namespace);

            var username = secret?.Data?.Username ?? throw new InvalidOperationException("Db user is empty.");
            var data = new Dictionary<string, string>
            {
                { "username", username },
                { "password", secret?.Data?.Password ?? throw new InvalidOperationException("Db password is empty.") }
            };

            var response = new VaultSecretResponse
            {
                Data = data,
                Renewable = secret.Renewable,
                LeaseDuration = secret.LeaseDurationSeconds,
                LeaseId = secret.LeaseId
            };

            logger.LogInformation($"Secret {request.Path} read successfully with username {response.Data["username"]} and lease ID {response.LeaseId}");

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
