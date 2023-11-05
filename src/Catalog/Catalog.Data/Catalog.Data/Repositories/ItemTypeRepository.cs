﻿namespace Catalog.Data.Repositories;

public class ItemTypeRepository: IItemTypeRepository
{
    private readonly CatalogContext _context;
    private readonly ILogger<ItemTypeRepository> _logger;

    public ItemTypeRepository(CatalogContext context, ILogger<ItemTypeRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateItemTypeAsync(ItemType itemType)
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

    public async Task<bool> DeleteItemTypeAsync(int id, bool useSoftDeleting)
    {
        try
        {
            var type = _context.ItemTypes.SingleOrDefault(x => x.Id == id);

            if (type is null)
                return false;

            if (useSoftDeleting)
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

    public async Task<ItemType?> GetItemTypeByIdAsync(int id)
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

    public async Task<IReadOnlyList<ItemType>> GetItemTypesAsync()
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

    public async Task<bool> UpdateItemTypeAsync(ItemType itemType)
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
