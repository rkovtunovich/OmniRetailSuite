namespace Infrastructure.DataManagement.IndexedDb;

public interface IDbDataService<T>
{
    Task AddItemAsync(string dbName, string storeName, T item);

    Task<T?> GetItemAsync(string dbName, string storeName, string key);

    Task<IEnumerable<T>> GetAllItemsAsync(string dbName, string storeName);

    Task UpdateItemAsync(string dbName, string storeName, T item);

    Task DeleteItemAsync(string dbName, string storeName, string key);

    Task ClearStoreAsync(string dbName, string storeName);
}
