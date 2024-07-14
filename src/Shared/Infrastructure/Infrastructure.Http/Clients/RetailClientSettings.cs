namespace Infrastructure.Http.Clients;

public class RetailClientSettings : HttpClientSettings
{
    public static readonly string Key = "HttpClients:RetailClient";

    //public static readonly string DefaultClientName = "RetailClient";
    //private static readonly string DefaultBaseAddress = "api/v1/retail/";
    //private static readonly string DefaultApiScope = "webappsgateway";

    public RetailClientSettings()
    {

    }
}
