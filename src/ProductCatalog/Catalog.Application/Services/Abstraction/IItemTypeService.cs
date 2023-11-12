namespace ProductCatalog.Application.Services.Abstraction;

public interface IItemTypeService
{
    Task<List<ItemTypeDto>> GetItemTypesAsync();

    Task<ItemTypeDto> GetItemTypeByIdAsync(Guid id);

    Task<ItemTypeDto> CreateItemTypeAsync(ItemTypeDto itemType);

    Task<ItemTypeDto> UpdateItemTypeAsync(ItemTypeDto itemType);

    Task DeleteItemTypeAsync(Guid id, bool isSoftDeleting);
}
