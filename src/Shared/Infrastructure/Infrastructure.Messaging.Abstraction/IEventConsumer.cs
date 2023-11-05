namespace Infrastructure.Messaging.Abstraction;

public interface IEventConsumer: IDisposable
{
    public void Configure(string consumerName);

    public void StartConsuming(CancellationToken cancellationToken);
}
