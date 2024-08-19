namespace RetailAssistant.Core.Models.Settings;

public class RetryPolicySettings
{
    public static readonly string Key = "RetryPolicy";

    public int MaxRetryAttempts { get; set; }

    public int Delay { get; set; }

}
