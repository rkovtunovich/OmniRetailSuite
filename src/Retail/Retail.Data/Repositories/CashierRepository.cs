namespace Retail.Data.Repositories;

public class CashierRepository(RetailDbContext context, ILogger<CashierRepository> logger) : IRetailRepository<Cashier>
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<CashierRepository> _logger = logger;

    public async Task<IEnumerable<Cashier>> GetEntitiesAsync()
    {
        try
        {
            return await _context.Cashiers
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting entities");
            throw;
        }
    }

    public async Task<Cashier?> GetEntityAsync(Guid id)
    {
        try
        {
            return await _context.Cashiers
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting entity with id {Id}", id);
            throw;
        }
    }

    public async Task<Cashier> AddEntityAsync(Cashier entity)
    {
        try
        {
            _context.Cashiers.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while adding {entity.GetType().Name}  with id {entity.Id}");
            throw;
        }
    }

    public async Task<Cashier> UpdateEntityAsync(Cashier entity)
    {
        try
        {
            _context.Cashiers.Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while updating {entity.GetType().Name} with id {entity.Id}");
            throw;
        }
    }

    public async Task DeleteEntityAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            var entity = await _context.Cashiers.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Entity with id {id} not found");

            if (isSoftDeleting)
            {
                entity.IsDeleted = true;
                _context.Cashiers.Update(entity);
            }
            else
            {
                _context.Cashiers.Remove(entity);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting entity with id {Id}", id);
            throw;
        }
    }
}
