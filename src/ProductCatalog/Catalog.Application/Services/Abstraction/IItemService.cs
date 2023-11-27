namespace ProductCatalog.Application.Services.Abstraction;

public interface IItemService
{
    Task<PaginatedItemsDto> GetItemsAsync(int pageSize, int pageIndex);

    Task<ItemDto?> GetItemByIdAsync(Guid id);

    Task<List<ItemDto>> GetItemsByNameAsync(string name);

    Task<PaginatedItemsDto> GetItemsByCategoryAsync(Guid? typeId, Guid? brandId);

    Task<PaginatedItemsDto> GetItemsByParentAsync(Guid parentId);

    Task<ItemDto> CreateItemAsync(ItemDto item);

    Task<ItemDto> UpdateItemAsync(ItemDto item);

    Task<bool> DeleteItemAsync(Guid id, bool isSoftDeleting);
}
