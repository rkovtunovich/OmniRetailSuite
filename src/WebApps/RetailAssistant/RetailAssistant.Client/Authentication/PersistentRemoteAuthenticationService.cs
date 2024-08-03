using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace RetailAssistant.Client.Authentication;

public class PersistentRemoteAuthenticationService<TRemoteAuthenticationState, TAccount> :
    RemoteAuthenticationService<TRemoteAuthenticationState, TAccount, OidcProviderOptions>
    where TRemoteAuthenticationState : RemoteAuthenticationState
    where TAccount : RemoteUserAccount
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<PersistentRemoteAuthenticationService<TRemoteAuthenticationState, TAccount>> _logger;

    public PersistentRemoteAuthenticationService(
        IJSRuntime jsRuntime,
        IOptionsSnapshot<RemoteAuthenticationOptions<OidcProviderOptions>> options,
        NavigationManager navigation,
        AccountClaimsPrincipalFactory<TAccount> accountClaimsPrincipalFactory,
        ILogger<PersistentRemoteAuthenticationService<TRemoteAuthenticationState, TAccount>> logger)
        : base(jsRuntime, options, navigation, accountClaimsPrincipalFactory, logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await GetPersistedUserAsync();
        if (user is null)
            return await base.GetAuthenticationStateAsync();

        return new AuthenticationState(user);
    }

    protected override async ValueTask<ClaimsPrincipal> GetAuthenticatedUser()
    {
        var user = await base.GetAuthenticatedUser();

        return user;
    }

    public async ValueTask<ClaimsPrincipal> GetPersistedUserAsync()
    {
        var serializedUser = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", PersistentStateAccountClaimsPrincipalFactory.AuthUserKey);

        if (string.IsNullOrEmpty(serializedUser))
            return new ClaimsPrincipal();

        if (JsonSerializer.Deserialize<RemoteUserAccount>(serializedUser) is not TAccount userAccount)
            return new ClaimsPrincipal();

        var serializedOptions = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", PersistentStateAccountClaimsPrincipalFactory.AuthOptionsKey);
        var options = JsonSerializer.Deserialize<RemoteAuthenticationUserOptions>(serializedOptions);
        if (options is null)
            return new ClaimsPrincipal();

        var user = await AccountClaimsPrincipalFactory.CreateUserAsync(userAccount, options);
        return user ?? new ClaimsPrincipal();
    }

    public override async Task<RemoteAuthenticationResult<TRemoteAuthenticationState>> SignOutAsync(RemoteAuthenticationContext<TRemoteAuthenticationState> context)
    {
        // Remove the user state from local storage
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", PersistentStateAccountClaimsPrincipalFactory.AuthUserKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", PersistentStateAccountClaimsPrincipalFactory.AuthOptionsKey);

        return await base.SignOutAsync(context);
    }
}
