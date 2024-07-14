namespace Infrastructure.SecretManagement.Abstraction;

public class SecretRequest
{
    public string Namespace { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public List<string> SecretKeys { get; set; } = [];

    public int Version { get; set; } = 0;

    public static string ConvertPathFromSnakeToKebabCase(string path)
    {
       return path.Replace('_', '-');
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Namespace, Path, SecretKeys, Version);
    }
}
