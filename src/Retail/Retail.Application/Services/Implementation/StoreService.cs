using Retail.Core.Repositories;

namespace Retail.Application.Services.Implementation;

public class StoreService(IStoreRepository storeRepository, ILogger<StoreService> logger) : IStoreService
{
    public async Task<IEnumerable<StoreDto>> GetStoresAsync()
    {
        try
        {
            var stores = await storeRepository.GetStoresAsync();

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
            var store = await storeRepository.GetStoreAsync(id);

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
            var store = await storeRepository.CreateStoreAsync(storeDto.ToEntity());

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
            var store = await storeRepository.UpdateStoreAsync(storeDto.ToEntity());

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
            await storeRepository.DeleteStoreAsync(id, isSoftDeleting);
        }
        catch (Exception)
        {
            logger.LogError("Error while deleting store with id {Id}", id);
            throw;
        }
    }
}
