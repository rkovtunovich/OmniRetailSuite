using Infrastructure.Messaging.Abstraction;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka;

public class EventConsumerHostedService : IHostedService
{
    private readonly IEventConsumer _eventConsumer;
    private readonly ILogger<EventConsumerHostedService> _logger;
    private Task? _consumingTask;
    private CancellationTokenSource _cts = null!;

    public EventConsumerHostedService(string consumerName, IEventConsumer eventConsumer, ILogger<EventConsumerHostedService> logger)
    {
        _eventConsumer = eventConsumer;
        _logger = logger;

        _eventConsumer.Configure(consumerName);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Kafka consumer");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            _consumingTask = Task.Run(() => _eventConsumer.StartConsuming(_cts.Token), cancellationToken);

            _logger.LogInformation("Kafka consumer started");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while starting Kafka consumer");
            throw;
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Kafka consumer");

        if (_consumingTask is null)      
            return;
        
        try
        {
            _cts.Cancel();

            // Wait for the consuming task to complete
            await Task.WhenAny(_consumingTask, Task.Delay(Timeout.Infinite, cancellationToken));
           
            _logger.LogInformation("Kafka consumer stopped");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while stopping Kafka consumer");
            _eventConsumer.Dispose();
            throw;
        }

        return;
    }
}
