namespace Contracts.Dtos.ProductCatalog;

public record BrandDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
