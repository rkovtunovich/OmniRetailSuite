namespace Infrastructure.Http.Uri;

public class ProductCatalogUriResolver(IOptions<ProductCatalogClientSettings> productCatalogClientSettings)
{
    public string GetAll<T>(int page = 0, int take = 1000)
    {
        var resourceName = GetResourceName<T>();

        return $"{GetBasePath()}{resourceName}?pageIndex={page}&pageSize={take}";
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

    #region Specials

    public string GetCatalogItemsByIds(string ids)
    {
        return $"{GetBasePath()}productitems/withids/{ids}";
    }

    public string GetCatalogItemsByParent(Guid parentId)
    {
        return $"{GetBasePath()}productitems/parent/{parentId}";
    }

    #endregion

    #region Private

    private static string GetResourceName<T>()
    {
        return $"{typeof(T).Name.TrimEnd("Dto".ToCharArray()).ToLower()}s";
    }

    private string GetBasePath()
    {
        return productCatalogClientSettings.Value.BasePath;
    }

    #endregion
}
