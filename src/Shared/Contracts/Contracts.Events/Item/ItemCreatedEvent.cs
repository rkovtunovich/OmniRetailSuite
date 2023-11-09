﻿namespace Contracts.Events.Item;

public record ItemCreatedEvent : IEvent
{
    public ItemCreatedEvent(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid EventId { get; init; } = Guid.NewGuid();

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public string EventType { get; init; } = nameof(ItemCreatedEvent);

    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
