using Contracts.Events;

namespace Infrastructure.Messaging.Abstraction;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
}
