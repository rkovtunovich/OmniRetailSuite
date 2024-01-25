namespace BackOffice.Core.Models.ExternalResources;

public abstract class ExternalResource
{
    protected ExternalResource(string clientName, string baseAddress, string apiScope)
    {
        ClientName = clientName;
        BaseAddress = baseAddress;
        ApiScope = apiScope;
    }

    public string ClientName { get; set; } = null!;

    public string BaseAddress { get; set; } = null!;

    public string ApiScope { get; set; } = null!;
}
