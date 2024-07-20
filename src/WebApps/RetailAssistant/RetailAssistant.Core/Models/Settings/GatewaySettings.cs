namespace RetailAssistant.Core.Models.Settings;

public class GatewaySettings
{
    public static readonly string Key = "WebGateway";

    public string BaseUrl { get; set; } = null!;

    public string HealthCheckPath { get; set; } = null!;

    public int HealthCheckTimeout { get; set; }

    public int HealthCheckInterval { get; set; }
}
