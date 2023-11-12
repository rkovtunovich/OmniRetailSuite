namespace Contracts.Dtos.ProductCatalog;

public record ItemTypeDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
