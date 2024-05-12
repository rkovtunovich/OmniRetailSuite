namespace Infrastructure.SecretManagement.Vault;

public record VaultSecretResponse
{
    public Dictionary<string, string> Data { get; init; } = [];

    public bool Renewable { get; init; }

    public int LeaseDuration { get; init; }

    public string LeaseId { get; init; }
}
