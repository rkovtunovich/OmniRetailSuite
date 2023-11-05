using Catalog.Application.DTOs.CatalogTDOs;

namespace Catalog.Application.Services.Abstraction;

public interface IItemService
{
    Task<PaginatedItemsDto> GetItemsAsync(int pageSize, int pageIndex);

    Task<ItemDto?> GetItemByIdAsync(int id);

    Task<List<ItemDto>> GetItemsByNameAsync(string name);

    Task<PaginatedItemsDto> GetItemsByCategoryAsync(int? typeId, int? brandId);

    Task<ItemDto> CreateItemAsync(ItemDto item);

    Task<ItemDto> UpdateItemAsync(ItemDto item);

    Task<bool> DeleteItemAsync(int id, bool isSoftDeleting);
}
