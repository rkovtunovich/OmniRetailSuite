using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Core.Repositories;

public interface IItemTypeRepository
{
    Task<IReadOnlyList<ProductType>> GetItemTypesAsync();

    Task<ProductType?> GetItemTypeByIdAsync(Guid id);

    Task<bool> CreateItemTypeAsync(ProductType itemType);

    Task<bool> UpdateItemTypeAsync(ProductType itemType);

    Task<bool> DeleteItemTypeAsync(Guid id, bool isSoftDeleting);
}
