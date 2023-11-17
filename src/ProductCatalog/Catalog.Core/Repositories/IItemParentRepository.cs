using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Core.Repositories;

public interface IItemParentRepository
{
    Task<bool> CreateItemParentAsync(ItemParent itemParent);

    Task<bool> DeleteItemParentAsync(Guid id, bool isSoftDeleting);

    Task<ItemParent?> GetItemParentAsync(Guid id);

    Task<List<ItemParent>> GetItemParentsAsync();

    Task<bool> UpdateItemParentAsync(ItemParent itemParent);
}
