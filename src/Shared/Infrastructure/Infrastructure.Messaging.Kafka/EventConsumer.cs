using Confluent.Kafka;
using Infrastructure.Messaging.Abstraction;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

public class EventConsumer : IEventConsumer
{
    private readonly ILogger<EventConsumer> _logger;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IOptions<KafkaSettings> _settings = null!;
    private readonly IEventWrapper _eventWrapper;
    private readonly IDeserializer<Message> _messageDeserializer = null!;

    private IConsumer<Ignore, Message> _consumer = null!;

    public EventConsumer(IEventWrapper eventWrapper, IEventDispatcher eventDispatcher, IDeserializer<Message> messageDeserializer, IOptions<KafkaSettings> settings, ILogger<EventConsumer> logger)
    {
        _logger = logger;
        _eventDispatcher = eventDispatcher;
        _settings = settings;
        _eventWrapper = eventWrapper;
        _messageDeserializer = messageDeserializer;
    }

    public void Configure(string consumerName)
    {
        try
        {
            var consumerConfig = _settings.Value?.Consumers?.FirstOrDefault(c => c.Name == consumerName) ?? throw new Exception($"Not defined consumer with name {consumerName} for {nameof(EventConsumer)}");

            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.Value.BootstrapServers,
                GroupId = consumerConfig.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, Message>(config)
                .SetValueDeserializer(_messageDeserializer)
                .Build();
            _consumer.Subscribe(consumerConfig.Topic);

            _logger.LogInformation($"Consumer {consumerName} configured");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error occurred during configuring consumer {consumerName}");
            throw;
        }
    }

    public void StartConsuming(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var message = consumeResult.Message.Value;
                if (message is null)
                {
                    _logger.LogWarning($"Message is null. Skipping...");
                    continue;
                }

                var @event = _eventWrapper.Unwrap(message);

                if (@event is null)
                {
                    _logger.LogWarning($"Event is null. Skipping...");
                    continue;
                }

                _eventDispatcher.Dispatch(@event);
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, "Error occurred during consuming message");
            }
        }
    }

    public void Dispose()
    {
        try
        {
            _consumer.Close();
            _consumer.Dispose();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during disposing consumer");
            throw;
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
