namespace ProductCatalog.Core.Repositories;

public interface IBrandRepository
{
    Task<IReadOnlyList<Brand>> GetBrandsAsync();

    Task<Brand?> GetBrandByIdAsync(Guid id);

    Task<bool> CreateBrandAsync(Brand brand);

    Task<bool> UpdateBrandAsync(Brand brand);

    Task<bool> DeleteBrandAsync(Guid id, bool useSoftDeleting);
}
