namespace ProductCatalog.Data.Repositories;

public class ProductItemRepository(ProductDbContext context, ILogger<ProductItemRepository> logger) : IItemRepository
{
    public async Task<bool> CreateItemAsync(ProductItem item)
    {
        try
        {
            context.Items.Add(item);

            if (item.ProductBrand is not null)
                context.Entry(item.ProductBrand).State = EntityState.Unchanged;

            if (item.ProductType is not null)
                context.Entry(item.ProductType).State = EntityState.Unchanged;

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(CreateItemAsync)}: {nameof(item)}: {item}", ex);
            throw;
        }
    }

    public async Task<ProductItem?> GetItemByIdAsync(Guid id)
    {
        try
        {
            var item = await context.Items
                .Include(c => c.ProductType)
                .Include(c => c.ProductBrand)
                .SingleOrDefaultAsync(ci => ci.Id == id);         

            return item;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(GetItemByIdAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<List<ProductItem>> GetItemsAsync(int pageSize, int pageIndex)
    {
        try
        {
            var itemsOnPage = await context.Items
                .Where(ci => !ci.IsDeleted)
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .Include(c => c.ProductType)
                .Include(c => c.ProductBrand)
                .ToListAsync();

            return itemsOnPage ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(GetItemsAsync)}: {nameof(pageSize)}: {pageSize}, {nameof(pageIndex)}: {pageIndex}", ex);
            throw;
        }
    }

    public async Task<List<ProductItem>> GetItemsByCategoryAsync(Guid? catalogBrandId, Guid? catalogTypeId)
    {
        try
        {
            if (catalogBrandId is null && catalogTypeId is null)
                return await context.Items
                    .Where(ci => !ci.IsDeleted)
                    .ToListAsync();

            var root = (IQueryable<ProductItem>)context.Items;

            if (catalogTypeId.HasValue)
                root = root.Where(ci => ci.ProductTypeId == catalogTypeId);

            if (catalogBrandId.HasValue)
                root = root.Where(ci => ci.ProductBrandId == catalogBrandId);

            var itemsOnPage = await root
                .ToListAsync();

            return itemsOnPage ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(GetItemsByCategoryAsync)}: {nameof(catalogBrandId)}: {catalogBrandId}, {nameof(catalogTypeId)}: {catalogTypeId}", ex);
            throw;
        }
    }

    public async Task<int> GetItemsCountAsync()
    {
        try
        {
            return await context.Items
                .Where(ci => !ci.IsDeleted)
                .CountAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error in {nameof(GetItemsCountAsync)}");
            throw;
        }
    }

    public Task<List<ProductItem>> GetItemsByIdAsync(IEnumerable<Guid> ids)
    {
        try
        {
            var items = context.Items.Where(ci => ids.Contains(ci.Id)).ToListAsync();

            return items;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(GetItemsByIdAsync)}: {nameof(ids)}: {ids}", ex);
            throw;
        }
    }

    public async Task<List<ProductItem>> GetItemsByNameAsync(string name)
    {
        try
        {
            var itemsOnPage = await context.Items
                .Where(c => c.Name.StartsWith(name))
                .ToListAsync();

            return itemsOnPage ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(GetItemsByNameAsync)}: {nameof(name)}: {name}", ex);
            throw;
        }
    }

    public async Task<List<ProductItem>> GetItemsByParentAsync(Guid catalogParentId)
    {
        try
        {
            var itemsOnPage = await context.Items
                .Where(ci => ci.ParentId == catalogParentId)
                .ToListAsync();

            return itemsOnPage ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(GetItemsByParentAsync)}: {nameof(catalogParentId)}: {catalogParentId}", ex);
            throw;
        }
    }

    public async Task<bool> UpdateItemAsync(ProductItem itemToUpdate)
    {
        try
        {
            context.Items.Update(itemToUpdate);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(UpdateItemAsync)}: {nameof(itemToUpdate)}: {itemToUpdate}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteItemAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            var product = context.Items.SingleOrDefault(x => x.Id == id);
            if (product is null)
                return false;

            if (isSoftDeleting)
            {
                product.IsDeleted = true;
                context.Items.Update(product);
                await context.SaveChangesAsync();
                return true;
            }

            context.Items.Remove(product);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in {nameof(DeleteItemAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }
}
