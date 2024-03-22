using BackOffice.Core.Models.Settings;
using IdentityModel.Client;

namespace BackOffice.Application.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly IOptions<IdentityServerSettings> _options;
    private readonly DiscoveryDocumentResponse _documentResponse;
    private readonly IHttpClientFactory _httpClientFactory;

    public TokenService(IOptions<IdentityServerSettings> options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _httpClientFactory = httpClientFactory;

        var httpClient = _httpClientFactory.CreateClient(ClientNames.IDENTITY);
        _documentResponse = httpClient.GetDiscoveryDocumentAsync(_options.Value.DiscoveryUrl).Result;

        if (_documentResponse.IsError)
            throw new Exception($"Unable to get discovery document {_documentResponse?.Error}", _documentResponse?.Exception);
    }

    public async Task<TokenResponse> GetToken(string scope)
    {
        var httpClient = _httpClientFactory.CreateClient(ClientNames.IDENTITY);
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = _documentResponse.TokenEndpoint,
            ClientId = _options.Value.ClientName,
            ClientSecret = _options.Value.ClientSecret,
            Scope = scope
        };
        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
            throw new Exception($"Unable to get a token {tokenResponse.Error}", tokenResponse.Exception);

        return tokenResponse;
    }
}
