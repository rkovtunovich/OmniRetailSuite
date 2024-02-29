namespace Infrastructure.Http.Uri;

public abstract class UrlResolverBase
{
    private static readonly string _suffix = "Dto";

    public virtual string GetResourceName<T>()
    {
        return $"{typeof(T).Name[..^_suffix.Length].ToLower()}s";
    }

    public virtual string GetAll<T>(int page = 0, int take = 1000)
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}?pageIndex={page}&pageSize={take}";
    }

    public virtual string Get<T>(Guid id)
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}/{id}";
    }

    public virtual string Add<T>()
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}";
    }

    public virtual string Update<T>()
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}";
    }

    public virtual string Delete<T>(Guid id, bool isSoftDeleting)
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}/{id}?useSoftDeleting={isSoftDeleting}";
    }

    protected abstract string GetBasePath();
}
