namespace Infrastructure.Http.Clients;

public class ProductCatalogClientSettings : HttpClientSettings
{
    public static readonly string Key = "HttpClients:ProductCatalogClient";

    //public static readonly string DefaultApiScope = "webappsgateway";
    //private static readonly string DefaultClientName = "ProductCatalogClient";
    //private static readonly string DefaultBaseAddress = "api/v1/productcatalog/";

    public ProductCatalogClientSettings() 
    {

    }
}
