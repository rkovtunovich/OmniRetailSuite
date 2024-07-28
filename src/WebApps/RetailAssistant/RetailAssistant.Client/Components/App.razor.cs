using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace RetailAssistant.Client.Components;

public partial class App
{
    //[Inject] private PersistentAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    //[Inject] private AccountClaimsPrincipalFactory<RemoteUserAccount> ClaimsPrincipalFactory { get; set; } = default!;

    //[Inject] private IAccessTokenProvider _accessTokenProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
         await Task.FromResult(0);

        //if (ClaimsPrincipalFactory is not PersistentStateAccountClaimsPrincipalFactory claimsPrincipalFactory)
        //    return;

        //var claimsPrincipal = await claimsPrincipalFactory.RestoreUserAsync();

        //// try to set the restored user as the current user
        //AuthenticationStateProvider.SetAuthenticationState(new AuthenticationState(claimsPrincipal));

    }
}
