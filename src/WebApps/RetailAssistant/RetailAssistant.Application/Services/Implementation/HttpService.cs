using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Infrastructure.Http;
using Infrastructure.Http.Clients;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace RetailAssistant.Application.Services.Implementation;

public class HttpService<TResource> : IHttpService<TResource> where TResource : HttpClientSettings, new()
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IDataSerializer _dataSerializer;
    private readonly ILogger<HttpService<TResource>> _logger;
    private readonly IOptions<TResource> _options;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public HttpService(IHttpClientFactory clientFactory, IDataSerializer dataSerializer, ILogger<HttpService<TResource>> logger, IAccessTokenProvider accessTokenProvider, IOptions<TResource> options)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _dataSerializer = dataSerializer;
        _accessTokenProvider = accessTokenProvider;
        _options = options;
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
            {
                string contentAsString = await responseMessage.Content.ReadAsStringAsync();
                throw new Exception($"Error post request {typeof(TResource).Name}) uri {uri}. {contentAsString}");
            }             
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
        var client = _clientFactory.CreateClient(_options.Value.Name);

        var tokenResponse = await _accessTokenProvider.RequestAccessToken();

        if (tokenResponse is null || tokenResponse.Status is not AccessTokenResultStatus.Success)
            throw new Exception($"Unable to get a token");

        if (!tokenResponse.TryGetToken(out var token))
            throw new Exception($"Unable to get a token");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

        return client;
    }
}
