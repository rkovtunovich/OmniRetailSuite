namespace ProductCatalog.Application.Services.Abstraction;

public interface IBrandService
{
    Task<List<ProductBrandDto>> GetBrandsAsync();

    Task<ProductBrandDto> GetBrandByIdAsync(Guid id);

    Task<ProductBrandDto> CreateBrandAsync(ProductBrandDto brand);

    Task<ProductBrandDto> UpdateBrandAsync(ProductBrandDto brand);

    Task DeleteBrandAsync(Guid id, bool isSoftDeleting);
}
