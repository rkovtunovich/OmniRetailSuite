namespace Contracts.Events.Item;

public record ItemDeletedEvent : IEvent
{
    public ItemDeletedEvent(Guid id)
    {
        Id = id;
    }

    public Guid EventId { get; init; } = Guid.NewGuid();

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public string EventType { get; init; } = nameof(ItemDeletedEvent);

    public Guid Id { get; init; }
}
