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
    public const string AuthUserKey = "authUser";
    public const string AuthOptionsKey = "authOptions";
    public const string AuthTokenKey = "authToken";
    private readonly string _sessionKey;

    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<PersistentRemoteAuthenticationService<TRemoteAuthenticationState, TAccount>> _logger;

    private bool _isTokenPersisted = false;
    private bool _isSessionRestored = false;

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

        _sessionKey = $"oidc.user:{Options.ProviderOptions.Authority}:{Options.UserOptions.AuthenticationType}";
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

    private async ValueTask<ClaimsPrincipal> GetPersistedUserAsync()
    {
        var serializedUser = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", AuthUserKey);

        if (string.IsNullOrEmpty(serializedUser))
            return new ClaimsPrincipal();

        if (JsonSerializer.Deserialize<RemoteUserAccount>(serializedUser) is not TAccount userAccount)
            return new ClaimsPrincipal();

        var serializedOptions = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", AuthOptionsKey);
        var options = JsonSerializer.Deserialize<RemoteAuthenticationUserOptions>(serializedOptions);
        if (options is null)
            return new ClaimsPrincipal();

        var user = await AccountClaimsPrincipalFactory.CreateUserAsync(userAccount, options);
        return user ?? new ClaimsPrincipal();
    }

    public override async Task<RemoteAuthenticationResult<TRemoteAuthenticationState>> SignOutAsync(RemoteAuthenticationContext<TRemoteAuthenticationState> context)
    {
        // Remove the user state from local storage
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AuthUserKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AuthOptionsKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", AuthTokenKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", _sessionKey);

        return await base.SignOutAsync(context);
    }

    public override async Task<RemoteAuthenticationResult<TRemoteAuthenticationState>> CompleteSignInAsync(RemoteAuthenticationContext<TRemoteAuthenticationState> context)
    {
        var result = await base.CompleteSignInAsync(context);

        // Store the user state in local storage

        _logger.LogInformation($"Sign in result: {result}");
        _logger.LogInformation("Saving user state to local storage");

        var user = await JsRuntime.InvokeAsync<TAccount>("AuthenticationService.getUser");
        var serializedUser = JsonSerializer.Serialize(user);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthUserKey, serializedUser);

        var serializedOptions = JsonSerializer.Serialize(Options.UserOptions);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthOptionsKey, serializedOptions);

        var session = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", _sessionKey);
        if (!string.IsNullOrEmpty(session))
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", _sessionKey, session);

        return result;
    }

    public override async ValueTask<AccessTokenResult> RequestAccessToken()
    {
        await RestoreSessionAsync();

        var tokenResult = await base.RequestAccessToken();

        if (tokenResult.Status == AccessTokenResultStatus.Success && !_isTokenPersisted)
        {
            if (!tokenResult.TryGetToken(out var token))
                return tokenResult;

            var serializedToken = JsonSerializer.Serialize(token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", AuthTokenKey, serializedToken);

            _isTokenPersisted = true;
        }
        // if status is not success, try retrieving the token from local storage
        else if (tokenResult.Status != AccessTokenResultStatus.Success)
        {
            var serializedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", AuthTokenKey);
            if (string.IsNullOrEmpty(serializedToken))
                return tokenResult;

            var token = JsonSerializer.Deserialize<AccessToken>(serializedToken);

            if (token is null)
                return tokenResult;

            tokenResult = new AccessTokenResult(AccessTokenResultStatus.Success, token, null, null);
        }

        return tokenResult;
    }

    private async ValueTask RestoreSessionAsync()
    {
        if (_isSessionRestored)
            return;

        var currentSession = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", _sessionKey);
        if (!string.IsNullOrEmpty(currentSession))
            return;

        var persistedSession = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", _sessionKey);
        if (!string.IsNullOrEmpty(persistedSession))
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", _sessionKey, persistedSession);

        _isSessionRestored = true;
    }
}
