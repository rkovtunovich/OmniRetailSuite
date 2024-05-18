using Contracts.Events.Item;
using Infrastructure.Messaging.Abstraction;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ItemService(IItemRepository itemRepository, IEventPublisher eventPublisher, ILogger<ItemService> logger) : IItemService
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private readonly ILogger<ItemService> _logger = logger;

    public async Task<List<ProductItemDto>> GetItemsAsync(int pageSize, int pageIndex)
    {
        var items = await _itemRepository.GetItemsAsync(pageSize, pageIndex);
        var catalogItemDtos = items.Select(x => x.ToDto()).ToList();

        return catalogItemDtos;
    }

    public async Task<ProductItemDto?> GetItemByIdAsync(Guid id)
    {
        var item = await _itemRepository.GetItemByIdAsync(id);

        if (item == null)
            return null;

        return item.ToDto();
    }

    public async Task<List<ProductItemDto>> GetItemsByNameAsync(string name)
    {
        var items = await _itemRepository.GetItemsByNameAsync(name);
        var itemsDtos = items.Select(x => x.ToDto()).ToList();

        itemsDtos = ChangeUriPlaceholder(itemsDtos);

        return itemsDtos;
    }

    public async Task<List<ProductItemDto>> GetItemsByCategoryAsync(Guid? typeId, Guid? brandId)
    {
        var root = await _itemRepository.GetItemsByCategoryAsync(typeId, brandId);

        var totalItems = root.Count;

        var itemsOnPage = root
            // TODO:
            //.Skip(pageSize * pageIndex)
            //.Take(pageSize)
            .Select(x => x.ToDto())
            .ToList();

        return itemsOnPage;
    }

    public async Task<List<ProductItemDto>> GetItemsByParentAsync(Guid parentId)
    {
        var items = await _itemRepository.GetItemsByParentAsync(parentId);
        var itemsDtos = items.Select(x => x.ToDto()).ToList();

        return itemsDtos;
    }

    public async Task<ProductItemDto> CreateItemAsync(ProductItemDto item)
    {
        var entity = item.ToEntity();
        var result = await _itemRepository.CreateItemAsync(entity);

        if (result)
            await _eventPublisher.PublishAsync(new ItemCreatedEvent(entity.Id, entity.Name, entity.CodeNumber, entity.CodePrefix));

        return item;
    }

    public async Task<ProductItemDto> UpdateItemAsync(ProductItemDto item)
    {
        var entity = item.ToEntity();
        var result = await _itemRepository.UpdateItemAsync(entity);

        if (result)
            await _eventPublisher.PublishAsync(new ItemUpdatedEvent(item.Id, item.Name, item.CodeNumber, item.CodePrefix));

        return item;
    }

    public async Task<bool> DeleteItemAsync(Guid id, bool isSoftDeleting)
    {
        var result = await _itemRepository.DeleteItemAsync(id, isSoftDeleting);

        if (result)
            await _eventPublisher.PublishAsync(new ItemDeletedEvent(id));

        return result;
    }

    private List<ProductItemDto> ChangeUriPlaceholder(List<ProductItemDto> items)
    {
        //var baseUri = _settings.PicBaseUrl;

        foreach (var item in items)
        {
            //item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
        }

        return items;
    }
}
