using System.Net.Http.Json;
using IdentityModel.Client;

namespace BackOffice.Application.Services.Implementation;

public class IdentityHttpService(IHttpClientFactory clientFactory,
                                 ITokenService tokenService,
                                 ILogger<IdentityHttpService> logger,
                                 IOptions<IdentityClientSettings> identityClientSettings,
                                 IDataSerializer dataSerializer) : IHttpService<IdentityClientSettings>
{
    public async Task<T?> GetAsync<T>(string uri)
    {
        try
        {
            var client = await GetClientAsync();

            logger.LogDebug($"Get request to identity service uri {client.BaseAddress}{uri}");

            var responseString = await client.GetStringAsync(uri) ?? throw new Exception($"Error getting request to identity service uri {uri}");

            if (string.IsNullOrEmpty(responseString))
                return default;

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

            var serializedData = dataSerializer.Serialize(data);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(uri, content);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception($"Error post request catalog uri {uri}. {responseMessage.Content}");

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
                throw new Exception($"Error post file catalog uri {uri}");
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
                throw new Exception($"Error put request catalog uri {uri}. {responseMessage.Content}");
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
                throw new Exception($"Error deleting request catalog uri {uri}. {responseMessage.Content}");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<HttpClient> GetClientAsync()
    {
        var client = clientFactory.CreateClient(ClientNames.IDENTITY);
        var tokenResponse = await tokenService.GetToken(identityClientSettings.Value.ApiScope);
        if (tokenResponse is null || tokenResponse.IsError)
            throw new Exception(tokenResponse?.Error ?? "Unable to get AccessToken");

        client.SetBearerToken(tokenResponse.AccessToken ?? string.Empty);

        return client;
    }
}
