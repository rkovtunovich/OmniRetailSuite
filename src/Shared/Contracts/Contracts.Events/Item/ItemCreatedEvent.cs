namespace Contracts.Events.Item;

public record ItemCreatedEvent : IEvent
{
    public ItemCreatedEvent(Guid id, string name, int codeNumber, string? codePrefix)
    {
        Id = id;
        Name = name;
        CodeNumber = codeNumber;
        CodePrefix = codePrefix;
    }

    public Guid EventId { get; init; } = Guid.NewGuid();

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public string EventType { get; init; } = nameof(ItemCreatedEvent);

    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }
}
