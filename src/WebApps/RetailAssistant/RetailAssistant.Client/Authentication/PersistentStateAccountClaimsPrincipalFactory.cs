﻿using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.JSInterop;

namespace RetailAssistant.Client.Authentication;

public class PersistentStateAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{ 
    public const string AuthUserKey = "authUser";
    public const string AuthOptionsKey = "authOptions";
    public const string AuthTokenKey = "authToken";

    private readonly IJSRuntime _jsRuntime;
    private IAccessTokenProvider _accessTokenProvider;

    public PersistentStateAccountClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor, IJSRuntime jsRuntime)
        : base(accessor)
    {
        _jsRuntime = jsRuntime;
        _accessTokenProvider = accessor.TokenProvider;
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);

        if(user?.Identity?.IsAuthenticated ?? false)
        {
            // Store the user state in local storage
            var serializedUser = JsonSerializer.Serialize(account);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthUserKey, serializedUser);

            var serializedOptions = JsonSerializer.Serialize(options);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthOptionsKey, serializedOptions);

            var token = await _accessTokenProvider.RequestAccessToken();
            var serializedToken = JsonSerializer.Serialize(token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthTokenKey, serializedToken);
        }

        return user ?? new ClaimsPrincipal();
    } 
}
