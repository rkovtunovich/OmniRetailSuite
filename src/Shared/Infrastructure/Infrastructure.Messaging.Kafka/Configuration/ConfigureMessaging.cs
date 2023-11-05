using Confluent.Kafka;
using Infrastructure.Messaging.Abstraction;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka.Configuration;

public static class ConfigureMessaging
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, KafkaSettings kafkaSettings)
    {
        services.AddSingleton<TopicManager>();
        services.AddSingleton<ISerializer<Message>, MessageSerializer>();
        services.AddSingleton<IDeserializer<Message>, MessageSerializer>();
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IEventConsumer, EventConsumer>();
        services.AddSingleton<IEventWrapper, EventWrapper>();
        services.AddSingleton<IEventDispatcher, EventDispatcher>();

        foreach (var consumerConfig in kafkaSettings.Consumers)
        {
            services.AddHostedService(provider =>
                new EventConsumerHostedService(consumerConfig.Name,
                                                    provider.GetRequiredService<IEventConsumer>(),
                                                    provider.GetRequiredService<ILogger<EventConsumerHostedService>>())
            );
        }

        return services;
    }
}
