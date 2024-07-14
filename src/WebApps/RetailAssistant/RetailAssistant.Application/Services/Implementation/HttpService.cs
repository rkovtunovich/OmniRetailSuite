using Infrastructure.Http;
using Infrastructure.Http.Clients;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RetailAssistant.Application.Services.Implementation;

public class HttpService<TResource>(IHttpClientFactory clientFactory,
                                    IDataSerializer dataSerializer,
                                    ILogger<HttpService<TResource>> logger,
                                    IAccessTokenProvider accessTokenProvider,
                                    IOptions<TResource> options) : IHttpService<TResource> where TResource : HttpClientSettings, new()
{
    public async Task<T?> GetAsync<T>(string uri)
    {
        try
        {
            var client = await GetClientAsync();
            var responseString = await client.GetStringAsync(uri) ?? throw new Exception($"Error getting request {typeof(TResource).Name}) uri {uri}");
            var value = dataSerializer.Deserialize<T>(responseString);

            return value;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task PutAsync<T>(string uri, T data)
    {
        try
        {
            var client = await GetClientAsync();
            var serializedData = dataSerializer.Serialize(data);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PutAsync(uri, content);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error put request {typeof(TResource).Name}) uri {uri}. {responseMessage.Content}");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<HttpClient> GetClientAsync()
    {
        var client = clientFactory.CreateClient(options.Value.Name);

        var tokenResponse = await accessTokenProvider.RequestAccessToken();

        if (tokenResponse is null || tokenResponse.Status is not AccessTokenResultStatus.Success)
            throw new Exception($"Unable to get a token");

        if (!tokenResponse.TryGetToken(out var token))
            throw new Exception($"Unable to get a token");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

        return client;
    }
}
