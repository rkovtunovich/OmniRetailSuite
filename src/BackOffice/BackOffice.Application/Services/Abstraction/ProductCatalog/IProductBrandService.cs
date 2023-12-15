using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Application.Services.Abstraction.ProductCatalog;

public interface IProductBrandService
{
    event Func<ProductBrand, Task> ProductBrandChanged;

    Task<List<ProductBrand>> GetBrandsAsync();

    Task<ProductBrand> GetBrandByIdAsync(Guid id);

    Task<ProductBrand> CreateBrandAsync(ProductBrand catalogBrand);

    Task<ProductBrand> UpdateBrandAsync(ProductBrand catalogBrand);

    Task DeleteBrandAsync(Guid id, bool isSoftDeleting);
}
