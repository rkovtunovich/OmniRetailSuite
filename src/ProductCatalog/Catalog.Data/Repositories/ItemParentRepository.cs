using ProductCatalog.Data;

namespace ProductCatalog.Data.Repositories;

public class ItemParentRepository : IItemParentRepository
{
    private readonly CatalogContext _context;
    private readonly ILogger<ItemParentRepository> _logger;

    public ItemParentRepository(CatalogContext context, ILogger<ItemParentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateItemParentAsync(ItemParent itemParent)
    {
        try
        {
            _context.ItemParents.Add(itemParent);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(CreateItemParentAsync)}: {nameof(itemParent)}: {itemParent}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteItemParentAsync(Guid id, bool useSoftDeleting)
    {
        try
        {
            var product = _context.ItemParents.SingleOrDefault(x => x.Id == id);
            if (product is null)
                return false;

            if (useSoftDeleting)
            {
                product.IsDeleted = true;
                _context.ItemParents.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }

            _context.ItemParents.Remove(product);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(DeleteItemParentAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<ItemParent?> GetItemParentAsync(Guid id)
    {
        try
        {
            var itemParent = await _context.ItemParents.SingleOrDefaultAsync(x => x.Id == id);

            return itemParent;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemParentAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<List<ItemParent>> GetItemParentsAsync()
    {
        try
        {
            // TODO: check when .Net 8.0 will be released
            //var items = await _catalogContext.ItemParentsRec().ToListAsync();

            var itemParents = await _context.ItemParents.ToListAsync();

            return itemParents;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemParentsAsync)}: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<bool> UpdateItemParentAsync(ItemParent itemParent)
    {
        try
        {
            _context.ItemParents.Update(itemParent);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(UpdateItemParentAsync)}: {nameof(itemParent)}: {itemParent}", ex);
            throw;
        }
    }
}
