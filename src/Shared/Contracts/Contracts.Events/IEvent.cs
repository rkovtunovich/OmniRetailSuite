﻿namespace Contracts.Events;

public interface IEvent
{
    Guid EventId { get; }

    DateTime OccurredOn { get; }

    string EventType { get; }
}
