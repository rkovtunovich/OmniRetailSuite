namespace Infrastructure.Http.Uri;

public class ProductCatalogUriResolver(IOptions<ProductCatalogClientSettings> productCatalogClientSettings) : UrlResolverBase
{
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

    protected override string GetBasePath()
    {
        return productCatalogClientSettings.Value.BasePath;
    }
}
