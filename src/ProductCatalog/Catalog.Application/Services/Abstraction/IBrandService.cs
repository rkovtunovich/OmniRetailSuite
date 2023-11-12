using Contracts.Dtos.ProductCatalog;

namespace ProductCatalog.Application.Services.Abstraction;

public interface IBrandService
{
    Task<List<BrandDto>> GetBrandsAsync();

    Task<BrandDto> GetBrandByIdAsync(Guid id);

    Task<BrandDto> CreateBrandAsync(BrandDto brand);

    Task<BrandDto> UpdateBrandAsync(BrandDto brand);

    Task DeleteBrandAsync(Guid id, bool isSoftDeleting);
}
