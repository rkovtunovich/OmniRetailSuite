namespace Contracts.Dtos.ProductCatalog;

public record PaginatedItemsDto
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public long Count { get; init; }

    public List<ProductItemDto> Data { get; init; } = [];
}
