using Microsoft.Extensions.Logging;

namespace BackOffice.Client.Services;

//public class CachedCatalogItemServiceDecorator : ICatalogItemService
//{
//    private readonly CatalogItemService _catalogItemService;
//    private ILogger<CachedCatalogItemServiceDecorator> _logger;

//    public CachedCatalogItemServiceDecorator(CatalogItemService catalogItemService, ILogger<CachedCatalogItemServiceDecorator> logger)
//    {
//        _catalogItemService = catalogItemService;
//        _logger = logger;
//    }

//    public async Task<List<CatalogItem>> ListPaged(int pageSize)
//    {
//        string key = "items";
//        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
//        if (cacheEntry != null)
//        {
//            _logger.LogInformation("Loading items from local storage.");
//            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
//            {
//                return cacheEntry.Value;
//            }
//            else
//            {
//                _logger.LogInformation($"Loading {key} from local storage.");
//                await _localStorageService.RemoveItemAsync(key);
//            }
//        }

//        var items = await _catalogItemService.ListPaged(pageSize);
//        var entry = new CacheEntry<List<CatalogItem>>(items);
//        await _localStorageService.SetItemAsync(key, entry);

//        var items = await _catalogItemService.ListPaged(pageSize);

//        return items;
//    }

//    public async Task<List<CatalogItem>> List()
//    {
//        string key = "items";
//        var cacheEntry = await _localStorageService.GetItemAsync<CacheEntry<List<CatalogItem>>>(key);
//        if (cacheEntry != null)
//        {
//            _logger.LogInformation("Loading items from local storage.");
//            if (cacheEntry.DateCreated.AddMinutes(1) > DateTime.UtcNow)
//            {
//                return cacheEntry.Value;
//            }
//            else
//            {
//                _logger.LogInformation($"Loading {key} from local storage.");
//                await _localStorageService.RemoveItemAsync(key);
//            }
//        }

//        var items = await _catalogItemService.List();
//        var entry = new CacheEntry<List<CatalogItem>>(items);
//        await _localStorageService.SetItemAsync(key, entry);

//        var items = await _catalogItemService.ListPaged(1);

//        return items;
//    }

//    public async Task<CatalogItem> GetById(int id)
//    {
//        return (await List()).FirstOrDefault(x => x.Id == id);
//    }

//    public async Task<CatalogItem> Create(CreateCatalogItemRequest catalogItem)
//    {
//        var result = await _catalogItemService.Create(catalogItem);

//        return result;
//    }

//    public async Task<CatalogItem> Edit(CatalogItem catalogItem)
//    {
//        var result = await _catalogItemService.Edit(catalogItem);

//        return result;
//    }

//    public async Task<string> Delete(int id)
//    {
//        var result = await _catalogItemService.Delete(id);

//        return result;
//    }
//}
