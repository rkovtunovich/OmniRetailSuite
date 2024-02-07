namespace Infrastructure.Http.Uri;

public static class CatalogUriHelper
{
    private static readonly ProductCatalogResource _productCatalogResource = new();

    public static string GetAll<T>(int page = 1, int take = 10)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_productCatalogResource.BaseAddress}{resourceName}?pageIndex={page}&pageSize={take}";
    }

    public static string Get<T>(Guid id)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_productCatalogResource.BaseAddress}{resourceName}/{id}";
    }

    public static string Add<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_productCatalogResource.BaseAddress}{resourceName}";
    }

    public static string Update<T>()
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_productCatalogResource.BaseAddress}{resourceName}";
    }

    public static string Delete<T>(Guid id, bool isSoftDeleting)
    {
        var resourceName = $"{typeof(T).Name.ToLower()}s";

        return $"{_productCatalogResource.BaseAddress}{resourceName}/{id}?useSoftDeleting={isSoftDeleting}";
    }

    #region Specials

    public static string GetCatalogItemsByIds(string ids)
    {
        return $"{_productCatalogResource.BaseAddress}productitems/withids/{ids}";
    }

    public static string GetCatalogItemsByParent(Guid parentId)
    {
        return $"{_productCatalogResource.BaseAddress}productitems/parent/{parentId}";
    }

    #endregion
}
