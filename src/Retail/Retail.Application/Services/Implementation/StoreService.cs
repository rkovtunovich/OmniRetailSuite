using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class StoreService(IRetailRepository<Store> storeRepository, IMapper mapper, ILogger<StoreService> logger) : IStoreService
{
    public async Task<IEnumerable<StoreDto>> GetStoresAsync()
    {
        try
        {
            var stores = await storeRepository.GetEntitiesAsync();

            return mapper.Map<IEnumerable<StoreDto>>(stores);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Error while getting stores");
            throw;
        }
    }

    public async Task<StoreDto?> GetStoreAsync(Guid id)
    {
        try
        {
            var store = await storeRepository.GetEntityAsync(id);

            return mapper.Map<StoreDto>(store);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error while getting store with id {id}");
            throw;
        }
    }

    public async Task<StoreDto> CreateStoreAsync(StoreDto storeDto)
    {
        try
        {
            var entity = mapper.Map<Store>(storeDto);
            var store = await storeRepository.AddEntityAsync(entity);

            return mapper.Map<StoreDto>(store);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating store");
            throw;
        }
    }

    public async Task<StoreDto> UpdateStoreAsync(StoreDto storeDto)
    {
        try
        {
            var entity = mapper.Map<Store>(storeDto);
            var store = await storeRepository.UpdateEntityAsync(entity);

            return mapper.Map<StoreDto>(store);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating store");
            throw;
        }
    }

    public async Task DeleteStoreAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await storeRepository.DeleteEntityAsync(id, isSoftDeleting);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting store with id {Id}", id);
            throw;
        }
    }
}
