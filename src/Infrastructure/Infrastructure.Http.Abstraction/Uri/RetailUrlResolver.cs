namespace Infrastructure.Http.Uri;

public class RetailUrlResolver(IOptions<RetailClientSettings> retailClientSettings)
{
    public string GetAll<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{retailClientSettings.Value.BasePath}{resourceName}";
    }

    public string Get<T>(Guid id)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{retailClientSettings.Value.BasePath}{resourceName}/{id}";
    }

    public string Add<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{retailClientSettings.Value.BasePath}{resourceName}";
    }

    public string Update<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{retailClientSettings.Value.BasePath}{resourceName}";
    }

    public string Delete<T>(Guid id, bool isSoftDeleting)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{retailClientSettings.Value.BasePath}{resourceName}/{id}?useSoftDeleting={isSoftDeleting}";
    }
}
