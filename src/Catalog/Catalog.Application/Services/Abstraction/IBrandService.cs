namespace Catalog.Application.Services.Abstraction;

public interface IBrandService
{
    Task<List<Brand>> GetBrandsAsync();

    Task<Brand> GetBrandByIdAsync(int id);

    Task<Brand> CreateBrandAsync(Brand brand);

    Task<Brand> UpdateBrandAsync(Brand brand);

    Task DeleteBrandAsync(int id, bool isSoftDeleting);
}
