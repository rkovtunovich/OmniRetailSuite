namespace Catalog.Data.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly CatalogContext _context;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(CatalogContext context, ILogger<ItemRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateItemAsync(Item item)
    {
        try
        {
            _context.Items.Add(item);

            if (item.CatalogBrand is not null)
                _context.Entry(item.CatalogBrand).State = EntityState.Unchanged;

            if (item.CatalogType is not null)
                _context.Entry(item.CatalogType).State = EntityState.Unchanged;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(CreateItemAsync)}: {nameof(item)}: {item}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteItemAsync(int id, bool useSoftDeleting)
    {
        try
        {
            var product = _context.Items.SingleOrDefault(x => x.Id == id);
            if (product is null)
                return false;

            if (useSoftDeleting)
            {
                product.IsDeleted = true;
                _context.Items.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }

            _context.Items.Remove(product);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(DeleteItemAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        try
        {
            var item = await _context.Items.SingleOrDefaultAsync(ci => ci.Id == id);

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemByIdAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<List<Item>> GetItemsAsync(int pageSize, int pageIndex)
    {
        try
        {
            var itemsOnPage = await _context.Items
                .Where(ci => !ci.IsDeleted)
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .Include(c => c.CatalogType)
                .Include(c => c.CatalogBrand)
                .ToListAsync();
        
            return itemsOnPage ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemsAsync)}: {nameof(pageSize)}: {pageSize}, {nameof(pageIndex)}: {pageIndex}", ex);
            throw;
        }
    }

    public async Task<List<Item>> GetItemsByCategoryAsync(int? catalogBrandId, int? catalogTypeId)
    {
        try
        {
            if(catalogBrandId is null && catalogTypeId is null)
                return await _context.Items
                    .Where(ci => !ci.IsDeleted)
                    .ToListAsync();

            var root = (IQueryable<Item>)_context.Items;

            if(catalogTypeId.HasValue)
                root = root.Where(ci => ci.CatalogTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)          
                root = root.Where(ci => ci.CatalogBrandId == catalogBrandId);           

            var itemsOnPage = await root
                .ToListAsync();

            return itemsOnPage ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemsByCategoryAsync)}: {nameof(catalogBrandId)}: {catalogBrandId}, {nameof(catalogTypeId)}: {catalogTypeId}", ex);
            throw;
        }
    }

    public async Task<int> GetItemsCountAsync()
    {
        try
        {
            return await _context.Items
                .Where(ci => !ci.IsDeleted)
                .CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"Error in {nameof(GetItemsCountAsync)}");
            throw;
        }
    }

    public Task<List<Item>> GetItemsByIdAsync(IEnumerable<int> ids)
    {
        try
        {
            var items = _context.Items.Where(ci => ids.Contains(ci.Id)).ToListAsync();

            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemsByIdAsync)}: {nameof(ids)}: {ids}", ex);
            throw;
        }
    }

    public async Task<List<Item>> GetItemsByNameAsync(string name)
    {
        try
        {
            var itemsOnPage = await _context.Items
                .Where(c => c.Name.StartsWith(name))
                .ToListAsync();

            return itemsOnPage ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemsByNameAsync)}: {nameof(name)}: {name}", ex);
            throw;
        }
    }

    public async Task<bool> UpdateItemAsync(Item item)
    {
        try
        {
            var catalogItem = await _context.Items.AsNoTracking().SingleOrDefaultAsync(i => i.Id == item.Id);

            if (catalogItem == null)
                return false;

            _context.Items.Update(catalogItem);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(UpdateItemAsync)}: {nameof(item)}: {item}", ex);
            throw;
        }
    }
}
