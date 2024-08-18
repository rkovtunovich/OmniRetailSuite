using Infrastructure.DataManagement.IndexedDb;
using Infrastructure.DataManagement.IndexedDb.Configuration.Settings;
using Microsoft.Extensions.Options;

namespace RetailAssistant.Data;

public class ApplicationRepository<TModel, TDbSettings> : IApplicationRepository<TModel, TDbSettings> where TModel : EntityModelBase where TDbSettings : DbSchema
{
    private readonly IDbDataService<TModel> _dbDataService = null!;
    private readonly ILogger<ApplicationRepository<TModel, TDbSettings>> _logger = null!;
    private readonly IOptions<TDbSettings> _options = null!;

    public ApplicationRepository(IDbDataService<TModel> dbDataService, ILogger<ApplicationRepository<TModel, TDbSettings>> logger, IOptions<TDbSettings> options)
    {
        _dbDataService = dbDataService;
        _logger = logger;
        _options = options;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        try
        {
            var result = await _dbDataService.GetAllRecordsAsync(_options.Value.Name, typeof(TModel).Name);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting all {typeof(TModel).Name} records.");
            throw;
        }
    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _dbDataService.GetRecordAsync(_options.Value.Name, typeof(TModel).Name, id.ToString());
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting {typeof(TModel).Name} record with id '{id}'.");
            throw;
        }
    }

    public async Task<bool> CreateAsync(TModel model)
    {
        try
        {
            await _dbDataService.AddRecordAsync(_options.Value.Name, typeof(TModel).Name, model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating {typeof(TModel).Name} record.");
            throw;
        }

        return true;
    }

    public async Task<bool> UpdateAsync(TModel model)
    {
        try
        {
            await _dbDataService.UpdateRecordAsync(_options.Value.Name, typeof(TModel).Name, model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating {typeof(TModel).Name} record.");
            throw;
        }

        return true;
    }

    public async Task<bool> CreateOrUpdateAsync(TModel model)
    {
        try
        {
            var result = await _dbDataService.GetRecordAsync(_options.Value.Name, typeof(TModel).Name, model.Id.ToString());
            if (result is not null) 
                await _dbDataService.UpdateRecordAsync(_options.Value.Name, typeof(TModel).Name, model);            
            else           
                await _dbDataService.AddRecordAsync(_options.Value.Name, typeof(TModel).Name, model);           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating or updating {typeof(TModel).Name} record.");
            throw;
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            await _dbDataService.DeleteRecordAsync(_options.Value.Name, typeof(TModel).Name, id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting {typeof(TModel).Name} record with id '{id}'.");
            throw;
        }

        return true;
    }
}
