namespace Catalog.Application.DTOs.CatalogTDOs;

public record PaginatedItemsDto
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public long Count { get; init; }

    public List<ItemDto> Data { get; init; } = new();

    public PaginatedItemsDto(int pageIndex, int pageSize, long count, List<ItemDto> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }
}
