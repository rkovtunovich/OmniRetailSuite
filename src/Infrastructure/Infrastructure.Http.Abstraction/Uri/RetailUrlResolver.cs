namespace Infrastructure.Http.Uri;

public class RetailUrlResolver(IOptions<RetailClientSettings> retailClientSettings)
{
    public string GetAll<T>()
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}";
    }

    public string Get<T>(Guid id)
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}/{id}";
    }

    public string Add<T>()
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}";
    }

    public string Update<T>()
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}";
    }

    public string Delete<T>(Guid id, bool isSoftDeleting)
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}/{id}?useSoftDeleting={isSoftDeleting}";
    }

    #region Private

    private static string GetResourceName<T>()
    {
        return $"{typeof(T).Name.TrimEnd("Dto".ToCharArray()).ToLower()}s";
    }

    private string GetBasePath()
    {
        return retailClientSettings.Value.BasePath;
    }

    #endregion
}
