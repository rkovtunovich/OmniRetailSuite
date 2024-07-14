namespace Retail.Data.Repositories;

public class AppClientSettingsRepository(RetailDbContext context, ILogger<AppClientSettingsRepository> logger) : IRetailRepository<AppClientSettings>
{
    private readonly RetailDbContext _context = context;
    private readonly ILogger<AppClientSettingsRepository> _logger = logger;

    public async Task<IEnumerable<AppClientSettings>> GetEntitiesAsync()
    {
        try
        {
            return await _context.AppClientSettings
                .ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while getting cashiers");
            throw;
        }
    }

    public async Task<AppClientSettings?> GetEntityAsync(Guid id)
    {
        try
        {
            return await _context.AppClientSettings
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting entity with id {Id}", id);
            throw;
        }
    }

    public async Task<AppClientSettings> AddEntityAsync(AppClientSettings entity)
    {
        try
        {
            _context.AppClientSettings.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error while adding {entity.GetType().Name} with id {entity.Id}");
            throw;
        }
    }

    public async Task<AppClientSettings> UpdateEntityAsync(AppClientSettings entity)
    {
        try
        {
            _context.AppClientSettings.Update(entity);

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
            var entity = await _context.AppClientSettings.FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Entity with id {id} not found");

            if (isSoftDeleting)
            {
                entity.IsDeleted = true;
                _context.AppClientSettings.Update(entity);
            }
            else
            {
                _context.AppClientSettings.Remove(entity);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error while deleting entity with id {Id}", id);
            throw;
        }
    }
}
