using Contracts.Events;
using Infrastructure.Serialization.Abstraction;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka;

public class EventWrapper: IEventWrapper
{
    private readonly IDataSerializer _dataSerializer;
    private readonly ILogger<EventWrapper> _logger;

    public EventWrapper(IDataSerializer eventSerializer, ILogger<EventWrapper> logger)
    {
        _dataSerializer = eventSerializer;
        _logger = logger;
    }

    public Message Wrap(IEvent @event)
    {
        try
        {
            var eventType = GetEventType(@event.EventType);
            var serializedEvent = _dataSerializer.Serialize(@event, eventType);

            return new Message(@event.EventType, serializedEvent);
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to wrap event {@event}", @event);
            throw;
        }
    }

    public IEvent Unwrap(Message message)
    {
        try
        {   
            var typeName = message.DataType;
            var eventType = GetEventType(typeName);

            if (eventType is null)
            {
                _logger?.LogError($"Failed to unwrap event {message}. Not found event type {typeName}");
                throw new Exception($"Failed to unwrap event {message}");
            }
            
            var @event = _dataSerializer.Deserialize(message.Payload, eventType);

            if (@event is null)
            {
                _logger?.LogError($"Failed to unwrap event {message}");
                throw new Exception($"Failed to unwrap event {message}");
            }

            return (IEvent)@event;
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to unwrap event {message}", message);
            throw;
        }
    }

    private Type GetEventType(string typeName)
    {
        var typePath = typeof(IEvent).Assembly.DefinedTypes.Where(x => x.Name == typeName).First().FullName;
        var assemblyName = typeof(IEvent).Assembly.GetName();
        var eventType = Type.GetType($"{typePath}, {assemblyName}");

        if (eventType is null)
        {
            _logger?.LogError($"Failed to unwrap event {typeName}. Not found event type {typePath}");
            throw new Exception($"Failed to unwrap event {typeName}");
        }

        return eventType;
    }
}
