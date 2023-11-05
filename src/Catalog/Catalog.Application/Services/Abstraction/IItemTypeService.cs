namespace Catalog.Application.Services.Abstraction;

public interface IItemTypeService
{
    Task<List<ItemType>> GetItemTypesAsync();

    Task<ItemType> GetItemTypeByIdAsync(int id);

    Task<ItemType> CreateItemTypeAsync(ItemType itemType);

    Task<ItemType> UpdateItemTypeAsync(ItemType itemType);

    Task DeleteItemTypeAsync(int id, bool isSoftDeleting);
}
