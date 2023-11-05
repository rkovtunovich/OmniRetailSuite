using Microsoft.Extensions.Options;

namespace BackOffice.Client.Services;

public class HttpService
{
    //private readonly IHttpClientFactory _httpClientFactory;
    //private readonly ToastService _toastService;
    //private readonly string _apiUrl;


    //public HttpService(IOptions<BaseUrlConfiguration> baseUrlConfiguration, ToastService toastService, IHttpClientFactory httpClientFactory)
    //{
    //    _toastService = toastService;
    //    _apiUrl = baseUrlConfiguration.Value.ApiBase;
    //    _httpClientFactory = httpClientFactory;
    //}

    //public async Task<T> HttpGet<T>(string uri) where T : class
    //{
    //    var httpClient = _httpClientFactory.CreateClient();

    //    var result = await httpClient.GetAsync($"{_apiUrl}{uri}");
    //    if (!result.IsSuccessStatusCode)
    //    {
    //        return null;
    //    }

    //    return await FromHttpResponseMessage<T>(result);
    //}

    //public async Task<T> HttpDelete<T>(string uri, int id) where T : class
    //{
    //    var httpClient = _httpClientFactory.CreateClient();

    //    var result = await httpClient.DeleteAsync($"{_apiUrl}{uri}/{id}");
    //    if (!result.IsSuccessStatusCode)
    //    {
    //        return null;
    //    }

    //    return await FromHttpResponseMessage<T>(result);
    //}

    //public async Task<T> HttpPost<T>(string uri, object dataToSend) where T : class
    //{
    //    var httpClient = _httpClientFactory.CreateClient();

    //    var content = ToJson(dataToSend);

    //    var result = await httpClient.PostAsync($"{_apiUrl}{uri}", content);
    //    if (!result.IsSuccessStatusCode)
    //    {
    //        var exception = JsonSerializer.Deserialize<ErrorDetails>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        });
    //        _toastService.ShowToast($"Error : {exception.Message}", ToastLevel.Error);

    //        return null;
    //    }

    //    return await FromHttpResponseMessage<T>(result);
    //}

    //public async Task<T> HttpPut<T>(string uri, object dataToSend) where T : class
    //{
    //    var httpClient = _httpClientFactory.CreateClient();

    //    var content = ToJson(dataToSend);

    //    var result = await httpClient.PutAsync($"{_apiUrl}{uri}", content);
    //    if (!result.IsSuccessStatusCode)
    //    {
    //        _toastService.ShowToast("Error", ToastLevel.Error);
    //        return null;
    //    }

    //    return await FromHttpResponseMessage<T>(result);
    //}

    //private StringContent ToJson(object obj)
    //{
    //    return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    //}

    //private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
    //{
    //    return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
    //    {
    //        PropertyNameCaseInsensitive = true
    //    });
    //}
}
