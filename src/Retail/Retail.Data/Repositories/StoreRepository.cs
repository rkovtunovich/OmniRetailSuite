namespace Retail.Data.Repositories;

public class StoreRepository(RetailDbContext context, ILogger<ReceiptRepository> logger): IStoreRepository
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<IEnumerable<Store>> GetStoresAsync()
    {
        try
        {
            return await _context.Stores.ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting stores");
            throw;
        }
    }

    public async Task<Store?> GetStoreAsync(Guid id)
    {
        try
        {
            return await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting store with id {Id}", id);
            throw;
        }
    }

    public async Task<Store> CreateStoreAsync(Store store)
    {
        try
        {
            _context.Stores.Add(store);

            await _context.SaveChangesAsync();

            return store;
        }
        catch (Exception)
        {
            _logger.LogError("Error while adding store");
            throw;
        }
    }

    public async Task<Store> UpdateStoreAsync(Store store)
    {
        try
        {
            _context.Stores.Update(store);

            await _context.SaveChangesAsync();

            return store;
        }
        catch (Exception)
        {
            _logger.LogError("Error while updating store");
            throw;
        }
    }

    public async Task DeleteStoreAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == id) ?? throw new Exception($"Store with id {id} not found");

            if (isSoftDeleting)
            {
                store.IsDeleted = true;

                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Stores.Remove(store);

                await _context.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting store with id {Id}", id);
            throw;
        }
    }

}
