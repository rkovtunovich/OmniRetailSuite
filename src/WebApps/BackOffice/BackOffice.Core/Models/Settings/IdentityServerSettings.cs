﻿namespace BackOffice.Core.Models.Settings;

public class IdentityServerSettings
{
    public string DiscoveryUrl { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public bool UseHttps { get; set; }

    public string Authority { get; set; } = null!;
}
