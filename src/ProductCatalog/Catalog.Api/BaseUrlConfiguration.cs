﻿namespace ProductCatalog.Api;

public class BaseUrlConfiguration
{
    public const string CONFIG_NAME = "baseUrls";

    public string ApiBase { get; set; } = string.Empty;

    public string WebBase { get; set; } = string.Empty;
}
