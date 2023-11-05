using Contracts.Events;

namespace Infrastructure.Messaging.Abstraction;

public interface IEventDispatcher
{
    void Dispatch(IEvent @event);
}
