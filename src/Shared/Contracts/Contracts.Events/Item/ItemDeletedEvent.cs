namespace Contracts.Events.Item;

public record ItemDeletedEvent : IEvent
{
    public ItemDeletedEvent(int id)
    {
        Id = id;
    }

    public Guid EventId { get; init; } = Guid.NewGuid();

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public string EventType { get; init; } = nameof(ItemDeletedEvent);

    public int Id { get; init; }
}
