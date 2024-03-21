namespace Infrastructure.SecretManagement.Vault;

public class VaultSecretManager : ISecretManager
{
    private readonly Dictionary<string, IGetSecretCommand> _commands = [];

    public VaultSecretManager(IVaultClient vaultClient)
    {
        _commands.Add("kv", new GetKeyValueSecretCommand(vaultClient));
        _commands.Add("database", new GetDatabaseSecretCommand(vaultClient));
    }

    public async Task<Dictionary<string, string>> GetSecretAsync(SecretRequest request)
    {
           if (!_commands.TryGetValue(request.Namespace, out var command))
                throw new InvalidOperationException($"No command found for namespace '{request.Namespace}'.");
    
            return await command.ExecuteAsync(request);
    }
}
