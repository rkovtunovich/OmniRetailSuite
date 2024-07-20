namespace RetailAssistant.Core.Models.Settings;

public class IdentityServerSettings
{
    public static readonly string Key = "IdentityServerSettings";

    public string DiscoveryUrl { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public bool UseHttps { get; set; }
}
