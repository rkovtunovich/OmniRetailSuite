using Microsoft.AspNetCore.Components.Authorization;

namespace RetailAssistant.Client.Extensions;

public static class AuthenticationStateProviderExtension
{
    public static async Task<string?> GetUserId(this AuthenticationStateProvider authenticationStateProvider)
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = state.User;

        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
