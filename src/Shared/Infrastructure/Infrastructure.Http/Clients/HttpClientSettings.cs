namespace Infrastructure.Http.Clients;

public abstract class HttpClientSettings()
{
    public string Name { get; set; } = null!;

    public string BasePath { get; set; } = null!;

    public string ApiScope { get; set; } = null!;
}
