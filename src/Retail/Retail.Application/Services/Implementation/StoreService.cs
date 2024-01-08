using Retail.Core.Entities;
using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class StoreService(IRetailRepository<Store> storeRepository, ILogger<StoreService> logger) : IStoreService
{
    public async Task<IEnumerable<StoreDto>> GetStoresAsync()
    {
        try
        {
            var stores = await storeRepository.GetEntitiesAsync();

            return stores.Select(s => s.ToDto()).ToList();
        }
        catch (Exception)
        {
            logger.LogError("Error while getting stores");
            throw;
        }
    }

    public async Task<StoreDto?> GetStoreAsync(Guid id)
    {
        try
        {
            var store = await storeRepository.GetEntityAsync(id);

            return store?.ToDto();
        }
        catch (Exception)
        {
            logger.LogError("Error while getting store with id {Id}", id);
            throw;
        }
    }

    public async Task<StoreDto> CreateStoreAsync(StoreDto storeDto)
    {
        try
        {
            var store = await storeRepository.AddEntityAsync(storeDto.ToEntity());

            return store.ToDto();
        }
        catch (Exception)
        {
            logger.LogError("Error while creating store");
            throw;
        }
    }

    public async Task<StoreDto> UpdateStoreAsync(StoreDto storeDto)
    {
        try
        {
            var store = await storeRepository.UpdateEntityAsync(storeDto.ToEntity());

            return store.ToDto();
        }
        catch (Exception)
        {
            logger.LogError("Error while updating store");
            throw;
        }
    }

    public async Task DeleteStoreAsync(Guid id, bool isSoftDeleting)
    {
        try
        {
            await storeRepository.DeleteEntityAsync(id, isSoftDeleting);
        }
        catch (Exception)
        {
            logger.LogError("Error while deleting store with id {Id}", id);
            throw;
        }
    }
}
