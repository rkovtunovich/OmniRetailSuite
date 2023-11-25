﻿namespace Contracts.Dtos.ProductCatalog;

public record ItemParentDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CodeNumber { get; init; }

    public string? CodePrefix { get; init; }

    public Guid? ParentId { get; set; }

    public IEnumerable<ItemParentDto>? Children { get; set; }
}
