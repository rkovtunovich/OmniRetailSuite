namespace BackOffice.Core.Models.ExternalResources;

public class RetailResource : ExternalResource
{
    private static readonly string DefaultClientName = "RetailClient";
    private static readonly string DefaultBaseAddress = "api/v1/retail/";
    private static readonly string DefaultApiScope = "webappsgateway";

    public RetailResource() : base(DefaultClientName, DefaultBaseAddress, DefaultApiScope)
    {

    }
}
