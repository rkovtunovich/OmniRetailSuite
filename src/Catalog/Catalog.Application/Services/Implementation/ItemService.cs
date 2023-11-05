using Catalog.Application.DTOs.CatalogTDOs;
using Catalog.Application.Services.Abstraction;
using Contracts.Events.Item;
using Infrastructure.Messaging.Abstraction;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Services.Implementation;

public class ItemService(IItemRepository itemRepository, IEventPublisher eventPublisher, ILogger<ItemService> logger) : IItemService
{
    private readonly IItemRepository _itemRepository = itemRepository;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private readonly ILogger<ItemService> _logger = logger;

    public async Task<PaginatedItemsDto> GetItemsAsync(int pageSize, int pageIndex)
    {
        var totalItems = await _itemRepository.GetItemsCountAsync();

        var items = await _itemRepository.GetItemsAsync(pageSize, pageIndex);
        var catalogItemDtos = items.AsQueryable().Select(ItemDto.Projection).ToList();

        catalogItemDtos = ChangeUriPlaceholder(catalogItemDtos);

        var paginatedItems = new PaginatedItemsDto(pageIndex, pageSize, totalItems, catalogItemDtos ?? []);

        return paginatedItems;
    }

    public async Task<ItemDto?> GetItemByIdAsync(int id)
    {
        var item = await _itemRepository.GetItemByIdAsync(id);

        return ItemDto.FromEntity(item);
    }

    public async Task<List<ItemDto>> GetItemsByNameAsync(string name)
    {
        var items = await _itemRepository.GetItemsByNameAsync(name);
        var itemsDtos = items.AsQueryable().Select(ItemDto.Projection).ToList();

        itemsDtos = ChangeUriPlaceholder(itemsDtos);

        return itemsDtos;
    }

    public async Task<PaginatedItemsDto> GetItemsByCategoryAsync(int? typeId, int? brandId)
    {
        var root = await _itemRepository.GetItemsByCategoryAsync(typeId, brandId);

        var totalItems = root.Count;

        var itemsOnPage = root.AsQueryable()
            // TODO:
            //.Skip(pageSize * pageIndex)
            //.Take(pageSize)
            .Select(ItemDto.Projection)
            .ToList();

        itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

        return new PaginatedItemsDto(0, totalItems, totalItems, itemsOnPage);
    }

    public async Task<ItemDto> CreateItemAsync(ItemDto item)
    {
        var entity = item.ToEntity();
        entity.CreatedAt = DateTime.UtcNow;
        var result = await _itemRepository.CreateItemAsync(entity);

        if (result)
            await _eventPublisher.PublishAsync(new ItemCreatedEvent(entity.Id, entity.Name));

        return item;
    }

    public async Task<ItemDto> UpdateItemAsync(ItemDto item)
    {
        var entity = item.ToEntity();
        entity.UpdatedAt = DateTime.UtcNow;
        var result = await _itemRepository.UpdateItemAsync(entity);

        if (result)
            await _eventPublisher.PublishAsync(new ItemUpdatedEvent(item.Id, item.Name));

        return item;
    }

    public async Task<bool> DeleteItemAsync(int id, bool isSoftDeleting)
    {
        var result = await _itemRepository.DeleteItemAsync(id, isSoftDeleting);

        if (result)
            await _eventPublisher.PublishAsync(new ItemDeletedEvent(id));

        return result;
    }

    private List<ItemDto> ChangeUriPlaceholder(List<ItemDto> items)
    {
        //var baseUri = _settings.PicBaseUrl;

        foreach (var item in items)
        {
            //item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);
        }

        return items;
    }
}
