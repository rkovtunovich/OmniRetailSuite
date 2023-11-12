using ProductCatalog.Data;

namespace ProductCatalog.Data.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly CatalogContext _context;
    private readonly ILogger<BrandRepository> _logger;

    public BrandRepository(CatalogContext context, ILogger<BrandRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateBrandAsync(Brand brand)
    {
        try
        {
            _context.Brands.Add(brand);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(CreateBrandAsync)}: {nameof(brand)}: {brand}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        try
        {
            var brand = _context.Brands.SingleOrDefault(x => x.Id == id);

            if (brand is null)
                return false;

            if (useSoftDeleting)
            {
                brand.IsDeleted = true;
                _context.Brands.Update(brand);
            }
            else
                _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(DeleteBrandAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<Brand?> GetBrandByIdAsync(Guid id)
    {
        try
        {
            var brand = await _context.Brands.SingleOrDefaultAsync(x => x.Id == id);

            return brand;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetBrandByIdAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<IReadOnlyList<Brand>> GetBrandsAsync()
    {
        try
        {
            var brands = await _context.Brands.ToListAsync();

            return brands;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in {nameof(GetBrandsAsync)}");
            throw;
        }
    }

    public async Task<bool> UpdateBrandAsync(Brand brand)
    {
        try
        {
            _context.Brands.Update(brand);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(UpdateBrandAsync)}: {nameof(brand)}: {brand}", ex);
            throw;
        }
    }
}
