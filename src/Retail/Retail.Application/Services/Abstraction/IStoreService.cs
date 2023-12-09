namespace Retail.Application.Services.Abstraction;

public interface IStoreService
{
    Task<StoreDto?> GetStoreAsync(Guid id);

    Task<IEnumerable<StoreDto>> GetStoresAsync();

    Task<StoreDto> CreateStoreAsync(StoreDto storeDto);

    Task<StoreDto> UpdateStoreAsync(StoreDto storeDto);

    Task DeleteStoreAsync(Guid id, bool isSoftDeleting);
}
