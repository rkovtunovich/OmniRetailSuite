namespace Infrastructure.SecretManagement.Abstraction;

public class SecretRequest
{
    public string Namespace { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public string SecretName { get; set; } = string.Empty;

    public int Version { get; set; } = 0;
}
