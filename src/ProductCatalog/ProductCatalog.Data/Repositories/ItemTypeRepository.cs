namespace ProductCatalog.Data.Repositories;

public class ItemTypeRepository : IItemTypeRepository
{
    private readonly ProductDbContext _context;
    private readonly ILogger<ItemTypeRepository> _logger;

    public ItemTypeRepository(ProductDbContext context, ILogger<ItemTypeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateItemTypeAsync(ProductType itemType)
    {
        try
        {
            _context.ItemTypes.Add(itemType);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(CreateItemTypeAsync)}: {nameof(itemType)}: {itemType}", ex);
            throw;
        }
    }

    public async Task<bool> DeleteItemTypeAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            var type = _context.ItemTypes.SingleOrDefault(x => x.Id == id);

            if (type is null)
                return false;

            if (isSoftDeleting)
            {
                type.IsDeleted = true;
                _context.ItemTypes.Update(type);
            }
            else
                _context.ItemTypes.Remove(type);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(DeleteItemTypeAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<ProductType?> GetItemTypeByIdAsync(Guid id)
    {
        try
        {
            var itemType = await _context.ItemTypes.SingleOrDefaultAsync(x => x.Id == id);
            return itemType;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(GetItemTypeByIdAsync)}: {nameof(id)}: {id}", ex);
            throw;
        }
    }

    public async Task<IReadOnlyList<ProductType>> GetItemTypesAsync()
    {
        try
        {
            var itemTypes = await _context.ItemTypes.ToListAsync();
            return itemTypes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in {nameof(GetItemTypesAsync)}");
            throw;
        }
    }

    public async Task<bool> UpdateItemTypeAsync(ProductType itemType)
    {
        try
        {
            _context.ItemTypes.Update(itemType);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in {nameof(UpdateItemTypeAsync)}: {nameof(itemType)}: {itemType}", ex);
            throw;
        }
    }
}
