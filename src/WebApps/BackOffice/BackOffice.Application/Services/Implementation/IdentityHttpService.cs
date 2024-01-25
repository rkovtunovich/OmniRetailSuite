using System.Net.Http.Json;
using BackOffice.Core.Models.ExternalResources;
using IdentityModel.Client;

namespace BackOffice.Application.Services.Implementation;

public class IdentityHttpService : IHttpService<IdentityResource>
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ITokenService _tokenService;
    private readonly ILogger<IdentityHttpService> _logger;

    public IdentityHttpService(IHttpClientFactory clientFactory, ITokenService tokenService, ILogger<IdentityHttpService> logger)
    {
        _clientFactory = clientFactory;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string uri)
    {
        try
        {
            var client = await GetClientAsync();
            var responseString = await client.GetStringAsync(uri) ?? throw new Exception($"Error getting request user preference uri {uri}");

            if (string.IsNullOrEmpty(responseString))
                return default;

            var value = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

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
        var client = _clientFactory.CreateClient(Constants.IDENTITY_CLIENT_NAME);
        var tokenResponse = await _tokenService.GetToken(Constants.IDENTITY_API_SCOPE);
        if (tokenResponse is null || tokenResponse.IsError)
            throw new Exception(tokenResponse?.Error ?? "Unable to get AccessToken");

        client.SetBearerToken(tokenResponse.AccessToken ?? string.Empty);

        return client;
    }
}
