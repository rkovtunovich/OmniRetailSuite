namespace RetailAssistant.Application.Helpers;

public static class IdentityUriHelper
{
    public static string GetPreferences(string userId)
    {
        return $"{Constants.IDENTITY_URI}userpreferences/{userId}";
    }

    public static string UpdatePreferences(string userId)
    {
        return $"{Constants.IDENTITY_URI}userpreferences/{userId}";
    }

    public static string GetRegisterUrl(string returnUrl)
    {
        return $"account/register?returnUrl={returnUrl}";
    }

    public static string ChangeBaseUrl(string? originalUrl, string newBaseUrl)
    {
        // Parse the original URL
        var originalUri = new Uri(originalUrl ?? string.Empty);

        // Combine the new base URL with the relative path and query from the original URL
        var newUrl = new Uri(new Uri(newBaseUrl), originalUri.PathAndQuery);

        return newUrl.ToString();
    }
}
