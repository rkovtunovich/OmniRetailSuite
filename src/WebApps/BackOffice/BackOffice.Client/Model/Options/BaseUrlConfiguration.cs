namespace BackOffice.Client.Model.Options;

public class BaseUrlConfiguration
{
    public const string CONFIG_NAME = "baseUrls";

    public string ApiBase { get; set; } = null!;

    public string WebBase { get; set; } = null!;
}
