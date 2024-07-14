namespace Retail.Data.Repositories;

public class ProductItemRepository(RetailDbContext context, ILogger<ReceiptRepository> logger) : ICatalogItemRepository
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<List<ProductItem>> GetProductItemsAsync()
    {
        try
        {
            return await _context.ProductItems.ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting product items");
            throw;
        }
    }

    public async Task<ProductItem?> GetProductItemAsync(Guid productItemId)
    {
        try
        {
            return await _context.ProductItems
                .FirstOrDefaultAsync(c => c.Id == productItemId);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting product item with id {Id}", productItemId);
            throw;
        }
    }

    public async Task<ProductItem> AddProductItemAsync(ProductItem productItem)
    {
        try
        {
            _context.ProductItems.Add(productItem);

            await _context.SaveChangesAsync();

            return productItem;
        }
        catch (Exception)
        {
            _logger.LogError("Error while adding product item with id {Id}", productItem.Id);
            throw;
        }
    }

    public async Task UpdateProductItemAsync(ProductItem productItem)
    {
        try
        {
            _context.ProductItems.Update(productItem);

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while updating product item with id {Id}", productItem.Id);
            throw;
        }
    }

    public async Task DeleteProductItemAsync(Guid productItemId, bool isSoftDeleting)
    {
        try
        {
            var productItem = await _context.ProductItems.FirstOrDefaultAsync(c => c.Id == productItemId) ?? throw new Exception($"Product item with id {productItemId} not found");

            if (isSoftDeleting)
            {
                productItem.IsDeleted = true;

                _context.ProductItems.Update(productItem);
            }
            else
            {
                _context.ProductItems.Remove(productItem);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting product item with id {Id}", productItemId);
            throw;
        }
    }
}
