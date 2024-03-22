using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaultSharp.V1.AuthMethods.Token;

namespace Infrastructure.SecretManagement.Vault.Configuration;

public static class ConfigureSecretManagement
{
    public static IServiceCollection AddSecretManagement(this IServiceCollection services, IConfiguration configuration)
    {
        var vaultSettings = configuration.GetSection(VaultSettings.SectionName).Get<VaultSettings>() 
            ?? throw new InvalidOperationException($"Configuration section '{VaultSettings.SectionName}' is missing.");

        var vaultClientSettings = new VaultClientSettings(vaultSettings.Address, new TokenAuthMethodInfo(vaultSettings.Token));

        services.AddSingleton<IVaultClient>(_ => new VaultClient(vaultClientSettings));
        services.AddSingleton<ISecretManager, VaultSecretManager>();

        return services;
    }
}
