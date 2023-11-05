using Contracts.Events;
using Infrastructure.Messaging.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging.Kafka;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventDispatcher> _logger;

    public EventDispatcher(IServiceProvider serviceProvider, ILogger<EventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Dispatch(IEvent @event)
    {
        try
        {
            var scope = _serviceProvider.CreateScope();

            var eventType = @event?.GetType() ?? throw new NullReferenceException(nameof(@event.GetType));
            var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var eventHandlers = scope.ServiceProvider.GetServices(eventHandlerType) ?? throw new NullReferenceException(nameof(eventHandlerType));

            foreach (var eventHandler in eventHandlers)
            {
                if (eventHandler == null)
                    continue;

                _logger.LogInformation($"Handling event: {eventType.Name}, id {@event.EventId}");

                var handleMethod = eventHandler.GetType().GetMethod(nameof(IEventHandler<IEvent>.Handle));
                handleMethod?.Invoke(eventHandler, new object[] { @event });
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to dispatch event {@event}", @event);
            throw;
        }
    }
}
