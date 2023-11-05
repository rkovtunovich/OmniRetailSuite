using Confluent.Kafka;
using Contracts.Events;
using Infrastructure.Messaging.Abstraction;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Messaging.Kafka;

public class EventPublisher : IEventPublisher
{
    private readonly string _topic;
    private readonly IProducer<Null, Message> _producer;
    private readonly IEventWrapper _eventWrapper;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IEventWrapper eventWrapper, ISerializer<Message> messageSerializer, IOptions<KafkaSettings> settings, ILogger<EventPublisher> logger)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers
        };

        _producer = new ProducerBuilder<Null, Message>(config)
            .SetValueSerializer(messageSerializer)
            .Build();

        _topic = settings.Value?.Producers?.Single(p => p.Name == nameof(EventPublisher))?.Topic
            ?? throw new Exception($"Not defined topic for {nameof(EventPublisher)}");

        _eventWrapper = eventWrapper;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        _logger.LogInformation($"Publishing event: {@event.GetType().Name}, id {@event.EventId}");

        var message = _eventWrapper.Wrap(@event);
        try
        {
            var deliveryResult = await _producer.ProduceAsync(_topic, new Message<Null, Message> { Value = message });

            if (deliveryResult.Status is PersistenceStatus.Persisted)
                _logger.LogInformation($"Published event: {@event.GetType().Name}, id {@event.EventId}");
            else
                _logger.LogError($"Delivery failed: id {@event.EventId}. {deliveryResult.Status}");
        }
        catch (ProduceException<Null, Message> e)
        {
            _logger.LogError($"Delivery failed: id {@event.EventId}. {e.Error.Reason}");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError($"Delivery failed: id {@event.EventId}. {e.Message}");
            throw;
        }
    }
}
