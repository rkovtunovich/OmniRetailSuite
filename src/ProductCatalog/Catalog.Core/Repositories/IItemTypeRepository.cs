namespace ProductCatalog.Core.Repositories;

public interface IItemTypeRepository
{
    Task<IReadOnlyList<ItemType>> GetItemTypesAsync();

    Task<ItemType?> GetItemTypeByIdAsync(Guid id);

    Task<bool> CreateItemTypeAsync(ItemType itemType);

    Task<bool> UpdateItemTypeAsync(ItemType itemType);

    Task<bool> DeleteItemTypeAsync(Guid id, bool useSoftDeleting);
}
