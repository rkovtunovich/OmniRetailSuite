using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace RetailAssistant.Application.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly IOptions<IdentityServerSettings> _options;
    private readonly IHttpClientFactory _httpClientFactory;

    private DiscoveryDocumentResponse? _documentResponse;

    public TokenService(IOptions<IdentityServerSettings> options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TokenResponse> GetToken(string scope)
    {
        await LoadDiscoveryDocument();

        var httpClient = _httpClientFactory.CreateClient(Constants.IDENTITY_CLIENT_NAME);
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = _documentResponse?.TokenEndpoint,
            ClientId = _options.Value.ClientName,
            ClientSecret = _options.Value.ClientSecret,
            Scope = scope
        };
        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
            throw new Exception($"Unable to get a token {tokenResponse.Error}", tokenResponse.Exception);

        return tokenResponse;
    }

    private async Task LoadDiscoveryDocument()
    {
        if (_documentResponse is not null)
            return;

        var httpClient = _httpClientFactory.CreateClient(Constants.IDENTITY_CLIENT_NAME);
        _documentResponse = await httpClient.GetDiscoveryDocumentAsync(_options.Value.DiscoveryUrl);

        if (_documentResponse.IsError)
            throw new Exception($"Unable to get discovery document {_documentResponse?.Error}", _documentResponse?.Exception);
    }
}
