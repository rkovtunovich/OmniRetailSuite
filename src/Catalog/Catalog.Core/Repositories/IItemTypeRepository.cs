namespace Catalog.Core.Repositories;

public interface IItemTypeRepository
{
    Task<IReadOnlyList<ItemType>> GetItemTypesAsync();

    Task<ItemType?> GetItemTypeByIdAsync(int id);

    Task<bool> CreateItemTypeAsync(ItemType itemType);

    Task<bool> UpdateItemTypeAsync(ItemType itemType);

    Task<bool> DeleteItemTypeAsync(int id, bool useSoftDeleting);
}
