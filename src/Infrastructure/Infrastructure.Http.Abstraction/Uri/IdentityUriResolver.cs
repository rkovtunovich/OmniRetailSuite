namespace Infrastructure.Http.Uri;

public class IdentityUriResolver(IOptions<IdentityClientSettings> identityClientOptions)
{
    public string GetPreferences(string userId)
    {
        return $"{identityClientOptions.Value.BasePath}userpreferences/{userId}";
    }

    public string UpdatePreferences(string userId)
    {
        return $"{identityClientOptions.Value.BasePath}userpreferences/{userId}";
    }

    public string GetRegisterUrl(string returnUrl)
    {
        return $"account/register?returnUrl={returnUrl}";
    }
}
