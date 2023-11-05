namespace Infrastructure.Messaging.Abstraction;

public interface IEventHandler<in TEvent> where TEvent : class
{
    void Handle(TEvent @event);
}
