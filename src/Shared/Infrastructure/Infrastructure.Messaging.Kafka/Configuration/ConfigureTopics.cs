using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.Kafka.Configuration;

public static class ConfigureTopics
{
    public static async Task EnsureKafkaTopicsCreated(this IHost host)
    {
        var topicManager = host.Services.GetService<TopicManager>() 
            ?? throw new NullReferenceException(nameof(TopicManager));

        await topicManager.EnsureTopicsCreatedAsync();
    }
}
