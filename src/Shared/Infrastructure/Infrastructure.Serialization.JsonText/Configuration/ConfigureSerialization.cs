using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Serialization.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Serialization.JsonText.Configuration;

public static class ConfigureSerialization
{
    public static IServiceCollection AddJsonTextSerialization(this IServiceCollection services, JsonSerializerOptions? serializerOptions = null)
    {
        var options = serializerOptions ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            
            Converters = 
            {
                new JsonStringEnumConverter()
            }
        };
        
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<IDataSerializer, MicrosoftJsonTextSerializer>();

        return services;
    }
}
