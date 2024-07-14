using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Core.Repositories;

public interface IItemParentRepository
{
    Task<bool> CreateItemParentAsync(ProductParent itemParent);

    Task<bool> DeleteItemParentAsync(Guid id, bool isSoftDeleting);

    Task<ProductParent?> GetItemParentAsync(Guid id);

    Task<List<ProductParent>> GetItemParentsAsync();

    Task<bool> UpdateItemParentAsync(ProductParent itemParent);
}
