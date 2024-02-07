using Infrastructure.Http.ExternalResources;

namespace Infrastructure.Http;

public interface IHttpService<TResource> where TResource : ExternalResource
{
    Task<T?> GetAsync<T>(string uri);

    Task PostAsync<T>(string uri, T data);

    Task PutAsync<T>(string uri, T data);

    Task DeleteAsync(string uri);

    Task PostFileAsync<T>(string uri, StreamContent file);
}
