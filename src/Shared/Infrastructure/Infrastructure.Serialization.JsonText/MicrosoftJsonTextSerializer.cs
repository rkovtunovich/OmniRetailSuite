using System.Text.Json;
using Infrastructure.Serialization.Abstraction;
using Microsoft.Extensions.Options;

namespace Infrastructure.Serialization.JsonText;

public class MicrosoftJsonTextSerializer(IOptions<JsonSerializerOptions> options) : IDataSerializer
{
    private readonly IOptions<JsonSerializerOptions> _options = options;

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, _options.Value);
    }

    public string Serialize(object obj, Type type)
    {
        return JsonSerializer.Serialize(obj, type, _options.Value);
    }

    public T? Deserialize<T>(string obj)
    {
        return JsonSerializer.Deserialize<T>(obj, _options.Value);
    }

    public object? Deserialize(string obj, Type type)
    {
        return JsonSerializer.Deserialize(obj, type, _options.Value);
    }
}
