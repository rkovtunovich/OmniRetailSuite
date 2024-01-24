using RetailAssistant.Client;

namespace RetailAssistant.Client.Helpers;

public static class IdentityUriHelper
{
    public static string GetPreferences(string userId)
    {
        return $"{Constants.IDENTITY_BASE_URI}userpreferences/{userId}";
    }

    public static string UpdatePreferences(string userId)
    {
        return $"{Constants.IDENTITY_BASE_URI}userpreferences/{userId}";
    }

    public static string GetRegisterUrl(string returnUrl)
    {
        return $"account/register?returnUrl={returnUrl}";
    }
}
