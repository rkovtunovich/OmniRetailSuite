namespace Infrastructure.Http.ExternalResources;

public abstract class ExternalResource(string clientName, string baseAddress, string apiScope)
{
    public string ClientName { get; set; } = clientName;

    public string BaseAddress { get; set; } = baseAddress;

    public string ApiScope { get; set; } = apiScope;
}
