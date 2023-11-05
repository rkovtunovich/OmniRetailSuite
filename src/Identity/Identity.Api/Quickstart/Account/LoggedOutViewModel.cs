// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Identity.Api.Quickstart.Account;

public class LoggedOutViewModel
{
    public string PostLogoutRedirectUri { get; set; } = null!;

    public string ClientName { get; set; } = string.Empty;

    public string SignOutIframeUrl { get; set; } = null!;

    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string LogoutId { get; set; } = string.Empty;

    public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;

    public string ExternalAuthenticationScheme { get; set; } = null!;
}
