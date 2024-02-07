namespace Infrastructure.Http.ExternalResources;

public class IdentityResource : ExternalResource
{
    public static readonly string DefaultApiScope = "IdentityServerApi";
    private static readonly string DefaultClientName = "IdentityClient";
    private static readonly string DefaultBaseAddress = "api/v1/identity/";

    public IdentityResource() : base(DefaultClientName, DefaultBaseAddress, DefaultApiScope)
    {

    }
}
