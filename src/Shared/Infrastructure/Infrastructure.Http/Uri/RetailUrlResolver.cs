namespace Infrastructure.Http.Uri;

public class RetailUrlResolver(IOptions<RetailClientSettings> retailClientSettings) : UrlResolverBase
{
    protected override string GetBasePath()
    {
        return retailClientSettings.Value.BasePath;
    }
}
