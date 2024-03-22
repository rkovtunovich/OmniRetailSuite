namespace Infrastructure.SecretManagement.Vault.Configuration.Settings;

public class VaultSettings
{
    public const string SectionName = "Vault";

    public string Address { get; set; } = null!;

    public string Token { get; set; } = null!; 
}
