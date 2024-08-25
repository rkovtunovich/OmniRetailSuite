namespace RetailAssistant.Core.Models.Settings;

public class IdentitySettings
{
    public static readonly string Key = "IdentitySettings";

    public string DiscoveryUrl { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public bool UseHttps { get; set; }
}
