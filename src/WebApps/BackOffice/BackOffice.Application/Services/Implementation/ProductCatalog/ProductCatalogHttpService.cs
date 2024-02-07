using System.Net.Http.Json;
using IdentityModel.Client;

namespace BackOffice.Application.Services.Implementation.ProductCatalog;

public class ProductCatalogHttpService : IHttpService<ProductCatalogResource>
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ITokenService _tokenService;
    private readonly IDataSerializer _dataSerializer;
    private readonly ILogger<ProductCatalogHttpService> _logger;

    public ProductCatalogHttpService(IHttpClientFactory clientFactory, ITokenService tokenService, IDataSerializer dataSerializer, ILogger<ProductCatalogHttpService> logger)
    {
        _clientFactory = clientFactory;
        _tokenService = tokenService;
        _logger = logger;
        _dataSerializer = dataSerializer;
    }

    public async Task<T?> GetAsync<T>(string uri)
    {
        try
        {
            var client = await GetClientAsync();
            var responseString = await client.GetStringAsync(uri) ?? throw new Exception($"Error getting request catalog uri {uri}");
            var value = _dataSerializer.Deserialize<T>(responseString);

            return value;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task PostAsync<T>(string uri, T data)
    {
        try
        {
            var client = await GetClientAsync();
            var responseMessage = await client.PostAsJsonAsync(uri, data);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error post request catalog uri {uri}. {responseMessage.Content}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task PostFileAsync<T>(string uri, StreamContent file)
    {
        try
        {
            var client = await GetClientAsync();
            var responseMessage = await client.PostAsync(uri, file);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error post file catalog uri {uri}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task PutAsync<T>(string uri, T data)
    {
        try
        {
            var client = await GetClientAsync();
            var responseMessage = await client.PutAsJsonAsync(uri, data);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error put request catalog uri {uri}. {responseMessage.Content}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(string uri)
    {
        try
        {
            var client = await GetClientAsync();
            var responseMessage = await client.DeleteAsync(uri);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error deleting request catalog uri {uri}. {responseMessage.Content}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<HttpClient> GetClientAsync()
    {
        var client = _clientFactory.CreateClient(ClientNames.PRODUCT_CATALOG);
        var tokenResponse = await _tokenService.GetToken(ProductCatalogResource.DefaultApiScope);
        if (tokenResponse is null || tokenResponse.IsError)
            throw new Exception(tokenResponse?.Error ?? "Unable to get AccessToken");

        client.SetBearerToken(tokenResponse.AccessToken ?? string.Empty);

        return client;
    }
}
