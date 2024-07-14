namespace ProductCatalog.Application.Services.Abstraction;

public interface IItemService
{
    Task<List<ProductItemDto>> GetItemsAsync(int pageSize, int pageIndex);

    Task<ProductItemDto?> GetItemByIdAsync(Guid id);

    Task<List<ProductItemDto>> GetItemsByNameAsync(string name);

    Task<List<ProductItemDto>> GetItemsByCategoryAsync(Guid? typeId, Guid? brandId);

    Task<List<ProductItemDto>> GetItemsByParentAsync(Guid parentId);

    Task<ProductItemDto> CreateItemAsync(ProductItemDto item);

    Task<ProductItemDto> UpdateItemAsync(ProductItemDto item);

    Task<bool> DeleteItemAsync(Guid id, bool isSoftDeleting);
}
