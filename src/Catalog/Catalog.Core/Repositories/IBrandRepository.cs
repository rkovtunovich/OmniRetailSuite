namespace Catalog.Core.Repositories;

public interface IBrandRepository
{
    Task<IReadOnlyList<Brand>> GetBrandsAsync();

    Task<Brand?> GetBrandByIdAsync(int id);

    Task<bool> CreateBrandAsync(Brand brand);

    Task<bool> UpdateBrandAsync(Brand brand);

    Task<bool> DeleteBrandAsync(int id, bool useSoftDeleting);
}
