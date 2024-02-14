namespace Infrastructure.Http.Uri;

public class ProductCatalogUriResolver(IOptions<ProductCatalogClientSettings> productCatalogClientSettings)
{
    public string GetAll<T>(int page = 1, int take = 10)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{productCatalogClientSettings.Value.BasePath}{resourceName}?pageIndex={page}&pageSize={take}";
    }

    public string Get<T>(Guid id)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{productCatalogClientSettings.Value.BasePath}{resourceName}/{id}";
    }

    public string Add<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{productCatalogClientSettings.Value.BasePath}{resourceName}";
    }

    public string Update<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{productCatalogClientSettings.Value.BasePath}{resourceName}";
    }

    public string Delete<T>(Guid id, bool isSoftDeleting)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{productCatalogClientSettings.Value.BasePath}{resourceName}/{id}?useSoftDeleting={isSoftDeleting}";
    }

    #region Specials

    public string GetCatalogItemsByIds(string ids)
    {
        return $"{productCatalogClientSettings.Value.BasePath}productitems/withids/{ids}";
    }

    public string GetCatalogItemsByParent(Guid parentId)
    {
        return $"{productCatalogClientSettings.Value.BasePath}productitems/parent/{parentId}";
    }

    #endregion
}
