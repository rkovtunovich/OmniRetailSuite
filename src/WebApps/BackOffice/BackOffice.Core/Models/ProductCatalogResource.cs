using BackOffice.Core.Models.ExternalResources;

namespace BackOffice.Core.Models;

public class ProductCatalogResource: ExternalResource
{
    private static readonly string DefaultClientName = "ProductCatalogClient";
    private static readonly string DefaultBaseAddress = "api/v1/productcatalog/";
    private static readonly string DefaultApiScope = "webappsgateway";

    public ProductCatalogResource() : base(DefaultClientName, DefaultBaseAddress, DefaultApiScope)
    {

    }
}
