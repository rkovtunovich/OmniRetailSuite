namespace Infrastructure.Http.Uri;

public static class IdentityUriHelper
{
    private static readonly IdentityResource _identityResource = new();

    public static string GetPreferences(string userId)
    {
        return $"{_identityResource.BaseAddress}userpreferences/{userId}";
    }

    public static string UpdatePreferences(string userId)
    {
        return $"{_identityResource.BaseAddress}userpreferences/{userId}";
    }

    public static string GetRegisterUrl(string returnUrl)
    {
        return $"account/register?returnUrl={returnUrl}";
    }
}
