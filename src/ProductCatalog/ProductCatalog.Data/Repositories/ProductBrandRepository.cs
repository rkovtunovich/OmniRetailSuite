namespace ProductCatalog.Data.Repositories;

public class ProductBrandRepository : IBrandRepository
{
    private readonly ProductDbContext _context;
    private readonly ILogger<ProductBrandRepository> _logger;

    public ProductBrandRepository(ProductDbContext context, ILogger<ProductBrandRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateBrandAsync(ProductBrand brand)
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

    public async Task<bool> DeleteBrandAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            var brand = _context.Brands.SingleOrDefault(x => x.Id == id);

            if (brand is null)
                return false;

            if (isSoftDeleting)
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

    public async Task<ProductBrand?> GetBrandByIdAsync(Guid id)
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

    public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
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

    public async Task<bool> UpdateBrandAsync(ProductBrand brand)
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
