namespace Infrastructure.Http.ExternalResources;

public class ProductCatalogResource : ExternalResource
{
    public static readonly string DefaultApiScope = "webappsgateway";
    private static readonly string DefaultClientName = "ProductCatalogClient";
    private static readonly string DefaultBaseAddress = "api/v1/productcatalog/";

    public ProductCatalogResource() : base(DefaultClientName, DefaultBaseAddress, DefaultApiScope)
    {

    }
}
