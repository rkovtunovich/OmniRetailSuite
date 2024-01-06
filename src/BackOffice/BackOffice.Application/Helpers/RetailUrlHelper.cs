namespace BackOffice.Application.Helpers;

public static class RetailUrlHelper
{
    private static readonly RetailResource _retailResource = new();

    public static string GetAll<T>()
    {
        var resourceName =  $"{typeof(T).Name.ToLower()}s";

        return $"{_retailResource.BaseAddress}{resourceName}";
    }

    public static string Get<T>(Guid id)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_retailResource.BaseAddress}{resourceName}/{id}";
    }

    public static string Add<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_retailResource.BaseAddress}{resourceName}";
    }

    public static string Update<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_retailResource.BaseAddress}{resourceName}";
    }

    public static string Delete<T>(Guid id, bool isSoftDeleting)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_retailResource.BaseAddress}{resourceName}/{id}?useSoftDeleting={isSoftDeleting}";
    }
}
