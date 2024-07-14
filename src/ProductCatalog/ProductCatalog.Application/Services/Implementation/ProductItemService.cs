using Contracts.Events.Item;
using Infrastructure.Messaging.Abstraction;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ProductItemService(IItemRepository itemRepository, IEventPublisher eventPublisher, IMapper mapper, ILogger<ProductItemService> logger) : IItemService
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private readonly ILogger<ProductItemService> _logger = logger;

    public async Task<List<ProductItemDto>> GetItemsAsync(int pageSize, int pageIndex)
    {
        var items = await _itemRepository.GetItemsAsync(pageSize, pageIndex);
        var catalogItemDtos = mapper.Map<List<ProductItemDto>>(items);

        return catalogItemDtos;
    }

    public async Task<ProductItemDto?> GetItemByIdAsync(Guid id)
    {
        var item = await _itemRepository.GetItemByIdAsync(id);

        if (item is null)
            return null;

        return mapper.Map<ProductItemDto>(item);
    }

    public async Task<List<ProductItemDto>> GetItemsByNameAsync(string name)
    {
        var items = await _itemRepository.GetItemsByNameAsync(name);
        var itemsDtos = mapper.Map<List<ProductItemDto>>(items);

        itemsDtos = ChangeUriPlaceholder(itemsDtos);

        return itemsDtos;
    }

    public async Task<List<ProductItemDto>> GetItemsByCategoryAsync(Guid? typeId, Guid? brandId)
    {
        var root = await _itemRepository.GetItemsByCategoryAsync(typeId, brandId);
        var itemsOnPage = mapper.Map<List<ProductItemDto>>(root);

        return itemsOnPage;
    }

    public async Task<List<ProductItemDto>> GetItemsByParentAsync(Guid parentId)
    {
        var items = await _itemRepository.GetItemsByParentAsync(parentId);
        var itemsDtos = mapper.Map<List<ProductItemDto>>(items);

        return itemsDtos;
    }

    public async Task<ProductItemDto> CreateItemAsync(ProductItemDto item)
    {
        var entity = mapper.Map<ProductItem>(item);
        var result = await _itemRepository.CreateItemAsync(entity);

        if (result)
            await _eventPublisher.PublishAsync(new ItemCreatedEvent(entity.Id, entity.Name, entity.CodeNumber, entity.CodePrefix));

        return item;
    }

    public async Task<ProductItemDto> UpdateItemAsync(ProductItemDto item)
    {
        var entity = mapper.Map<ProductItem>(item);
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
