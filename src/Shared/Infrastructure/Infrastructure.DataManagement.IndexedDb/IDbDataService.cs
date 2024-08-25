namespace Infrastructure.DataManagement.IndexedDb;

public interface IDbDataService<TRecord>
{
    Task AddRecordAsync(string dbName, string storeName, TRecord item);

    Task<TRecord?> GetRecordAsync(string dbName, string storeName, string key);

    Task<IEnumerable<TRecord>> GetAllRecordsAsync(string dbName, string storeName);

    Task<IEnumerable<TRecord>> GetAllRecordsByIndexAsync(string dbName, string storeName, string indexName, string indexValue);

    Task UpdateRecordAsync(string dbName, string storeName, TRecord item);

    Task DeleteRecordAsync(string dbName, string storeName, string key);

    Task ClearStoreAsync(string dbName, string storeName);
}
