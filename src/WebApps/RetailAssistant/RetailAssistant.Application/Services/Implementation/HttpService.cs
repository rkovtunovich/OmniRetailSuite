using System.Net.Http.Json;
using System.Net.Http.Headers;
using RetailAssistant.Core.Models.ExternalResources;

namespace RetailAssistant.Application.Services.Implementation;

public class HttpService<TResource> : IHttpService<TResource> where TResource : ExternalResource, new()
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ITokenService _tokenService;
    private readonly IDataSerializer _dataSerializer;
    private readonly ILogger<HttpService<TResource>> _logger;
    private readonly TResource _resource = new();

    public HttpService(IHttpClientFactory clientFactory, ITokenService tokenService, IDataSerializer dataSerializer, ILogger<HttpService<TResource>> logger)
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
            var responseString = await client.GetStringAsync(uri) ?? throw new Exception($"Error getting request {typeof(TResource).Name}) uri {uri}");
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
                throw new Exception($"Error post request {typeof(TResource).Name}) uri {uri}. {responseMessage.Content}");

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
                throw new Exception($"Error post request {typeof(TResource).Name}) uri {uri}. {responseMessage.Content}");

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
                throw new Exception($"Error put request {typeof(TResource).Name}) uri {uri}. {responseMessage.Content}");

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
                throw new Exception($"Error delete request {typeof(TResource).Name}) uri {uri}. {responseMessage.Content}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<HttpClient> GetClientAsync()
    {
        var client = _clientFactory.CreateClient(_resource.ClientName);
        var tokenResponse = await _tokenService.GetToken(_resource.ApiScope);
        if (tokenResponse is null || tokenResponse.IsError)
            throw new Exception(tokenResponse?.Error ?? "Unable to get AccessToken");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken ?? string.Empty);

        return client;
    }
}
