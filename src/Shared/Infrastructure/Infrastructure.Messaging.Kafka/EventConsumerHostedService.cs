using Infrastructure.Messaging.Abstraction;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka;

public class EventConsumerHostedService : IHostedService
{
    private readonly IEventConsumer _eventConsumer;
    private readonly ILogger<EventConsumerHostedService> _logger;

    public EventConsumerHostedService(string consumerName, IEventConsumer eventConsumer, ILogger<EventConsumerHostedService> logger)
    {
        _eventConsumer = eventConsumer;
        _logger = logger;

        _eventConsumer.Configure(consumerName);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Kafka consumer");

        try
        {
            _eventConsumer.StartConsuming(cancellationToken);

            _logger.LogInformation("Kafka consumer started");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while starting Kafka consumer");
            throw;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Kafka consumer");

        try
        {
            _eventConsumer.Dispose();

            _logger.LogInformation("Kafka consumer stopped");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while stopping Kafka consumer");
            throw;
        }

        return Task.CompletedTask;
    }
}
