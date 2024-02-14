﻿using Infrastructure.Http.Uri;
using Microsoft.AspNetCore.Components.Authorization;

namespace BackOffice.Client.Shared;

public partial class LoginDisplay
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    [Inject]
    public IConfiguration? Configuration { get; set; }

    [Inject]
    public IdentityUriResolver IdentityUriResolver { get; set; } = null!;

    private string ShowUserName(AuthenticationState context)
    {
        var name = context?.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Anonymous";
        return name;
    }

    private string GetLoginUrl()
    {
        if (Configuration is null)
            throw new ArgumentNullException(nameof(Configuration));

        var identityUrl = Configuration.GetValue<string>("WebGateway"); 

        return identityUrl + "/" + IdentityUriResolver.GetRegisterUrl(NavigationManager?.Uri ?? "/");
    }
}
