using System.Net;
using VaultSharp.Core;

namespace Infrastructure.SecretManagement.Vault;

public class GetDatabaseSecretCommand : IGetSecretCommand
{
    private readonly IVaultClient _vaultClient;

    public GetDatabaseSecretCommand(IVaultClient vaultClient)
    {
        _vaultClient = vaultClient;
    }

    public async Task<Dictionary<string, string>> ExecuteAsync(SecretRequest request)
    {
        try
        {
            var secret = await _vaultClient.V1.Secrets.Database.GetCredentialsAsync(request.Path, mountPoint: request.Namespace);

            var result = new Dictionary<string, string>
            {
                { "username", secret?.Data?.Username ?? throw new InvalidOperationException("Db user is empty.") },
                { "password", secret?.Data?.Password ?? throw new InvalidOperationException("Db password is empty.") }
            };

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
