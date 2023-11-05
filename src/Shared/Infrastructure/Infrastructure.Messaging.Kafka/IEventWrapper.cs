using Contracts.Events;

namespace Infrastructure.Messaging.Kafka;

public interface IEventWrapper
{
    Message Wrap(IEvent @event);

    IEvent Unwrap(Message message);
}
