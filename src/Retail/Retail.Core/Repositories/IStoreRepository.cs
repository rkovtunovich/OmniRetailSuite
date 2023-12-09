namespace Retail.Core.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetStoreAsync(Guid id);

    Task<IEnumerable<Store>> GetStoresAsync();

    Task<Store> CreateStoreAsync(Store store);

    Task<Store> UpdateStoreAsync(Store store);

    Task DeleteStoreAsync(Guid id, bool isSoftDeleting);
}
