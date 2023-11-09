namespace Retail.Data.Repositories;

public class CatalogItemRepository(RetailDbContext context, ILogger<ReceiptRepository> logger) : ICatalogItemRepository
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<List<CatalogItem>> GetCatalogItemsAsync()
    {
        try
        {
            return await _context.CatalogItems.ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting catalog items");
            throw;
        }
    }

    public async Task<CatalogItem?> GetCatalogItemAsync(Guid receiptItemId)
    {
        try
        {
            return await _context.CatalogItems
                .FirstOrDefaultAsync(c => c.Id == receiptItemId);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting catalog item with id {Id}", receiptItemId);
            throw;
        }
    }

    public async Task<CatalogItem> AddCatalogItemAsync(CatalogItem catalogItem)
    {
        try
        {
            _context.CatalogItems.Add(catalogItem);

            await _context.SaveChangesAsync();

            return catalogItem;
        }
        catch (Exception)
        {
            _logger.LogError("Error while adding catalog item with id {Id}", catalogItem.Id);
            throw;
        }
    }

    public async Task UpdateCatalogItemAsync(CatalogItem catalogItem)
    {
        try
        {
            _context.CatalogItems.Update(catalogItem);

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while updating catalog item with id {Id}", catalogItem.Id);
            throw;
        }
    }

    public async Task DeleteCatalogItemAsync(Guid catalogItemId, bool useSoftDeleting)
    {
        try
        {
            var catalogItem = await _context.CatalogItems.FirstOrDefaultAsync(c => c.Id == catalogItemId) ?? throw new Exception($"Catalog item with id {catalogItemId} not found");

            if (useSoftDeleting)
            {
                catalogItem.IsDeleted = true;

                _context.CatalogItems.Update(catalogItem);
            }
            else
            {
                _context.CatalogItems.Remove(catalogItem);
            }
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting catalog item with id {Id}", catalogItemId);
            throw;
        }
    }
}
