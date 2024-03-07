namespace Retail.Data.Repositories;

public class StoreRepository(RetailDbContext context, ILogger<ReceiptRepository> logger): IRetailRepository<Store>
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<ReceiptRepository> _logger = logger;

    public async Task<IEnumerable<Store>> GetEntitiesAsync()
    {
        try
        {
            return await _context.Stores.Include(s => s.Cashiers).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting stores");
            throw;
        }
    }

    public async Task<Store?> GetEntityAsync(Guid id)
    {
        try
        {
            return await _context.Stores
                .Include(s => s.Cashiers)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting store with id {Id}", id);
            throw;
        }
    }

    public async Task<Store> AddEntityAsync(Store store)
    {
        try
        {
            _context.Stores.Add(store);

            await _context.SaveChangesAsync();

            return store;
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error while adding store");
            throw;
        }
    }

    public async Task<Store> UpdateEntityAsync(Store store)
    {
        try
        {
            // Load the existing store including associated cashiers
            var existingStore = await _context.Stores
                .Include(s => s.Cashiers)
                .FirstOrDefaultAsync(s => s.Id == store.Id) ?? throw new InvalidOperationException("Store not found.");

            // Update store properties
            _context.Entry(existingStore).CurrentValues.SetValues(store);

            // Update many-to-many relationship
            var existingCashiers = existingStore.Cashiers.ToList();
            foreach (var cashier in existingCashiers)
            {
                if (!store.Cashiers.Any(c => c.Id == cashier.Id))             
                    existingStore.Cashiers.Remove(cashier);           
            }

            foreach (var cashier in store.Cashiers)
            {
                if (!existingCashiers.Any(c => c.Id == cashier.Id))               
                    existingStore.Cashiers.Add(cashier);             
            }

            // Save changes
            await _context.SaveChangesAsync();

            return existingStore;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating store");
            throw;
        }
    }

    public async Task DeleteEntityAsync(Guid id, bool isSoftDeleting)
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
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting store with id {Id}", id);
            throw;
        }
    }

}
