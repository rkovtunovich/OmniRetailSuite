using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Core.Repositories;

public interface IBrandRepository
{
    Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

    Task<ProductBrand?> GetBrandByIdAsync(Guid id);

    Task<bool> CreateBrandAsync(ProductBrand brand);

    Task<bool> UpdateBrandAsync(ProductBrand brand);

    Task<bool> DeleteBrandAsync(Guid id, bool isSoftDeleting);
}
