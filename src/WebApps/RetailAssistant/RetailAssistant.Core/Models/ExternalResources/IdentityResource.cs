namespace RetailAssistant.Core.Models.ExternalResources;

public class IdentityResource : ExternalResource
{
    public static readonly string DefaultClientName = "IdentityClient";
    private static readonly string DefaultBaseAddress = "api/v1/identity/";
    private static readonly string DefaultApiScope = "IdentityServerApi";//"webappsgateway";

    public IdentityResource() : base(DefaultClientName, DefaultBaseAddress, DefaultApiScope)
    {

    }
}
