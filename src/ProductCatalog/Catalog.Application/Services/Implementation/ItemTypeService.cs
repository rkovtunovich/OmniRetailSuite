using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ItemTypeService: IItemTypeService
{
    private readonly IItemTypeRepository _itemTypeRepository;
    private readonly ILogger<ItemTypeService> _logger;

    public ItemTypeService(IItemTypeRepository itemTypeRepository, ILogger<ItemTypeService> logger)
    {
        _itemTypeRepository = itemTypeRepository;
        _logger = logger;
    }

    public async Task<List<ItemTypeDto>> GetItemTypesAsync()
    {
        var itemTypes = await _itemTypeRepository.GetItemTypesAsync();
        return itemTypes.Select(x => x.ToDto()).ToList();
    }

    public async Task<ItemTypeDto> GetItemTypeByIdAsync(Guid id)
    {
        var itemType = await _itemTypeRepository.GetItemTypeByIdAsync(id);
        if (itemType is null)
        {
            _logger.LogError($"ItemType with id: {id} not found.");
            throw new Exception($"ItemType with id: {id} not found.");
        }

        return itemType.ToDto();
    }

    public async Task<ItemTypeDto> CreateItemTypeAsync(ItemTypeDto itemTypeDto)
    {
        var itemType = itemTypeDto.ToEntity();
        var result = await _itemTypeRepository.CreateItemTypeAsync(itemType);
        if (!result)
        {
            _logger.LogError($"ItemType with name: {itemTypeDto.Name} not created.");
            throw new Exception($"ItemType with name: {itemTypeDto.Name} not created.");
        }

        return itemType.ToDto();
    }

    public async Task<ItemTypeDto> UpdateItemTypeAsync(ItemTypeDto itemTypeDto)
    {
        var itemType = itemTypeDto.ToEntity();
        var result = await _itemTypeRepository.UpdateItemTypeAsync(itemType);
        if (!result)
        {
            _logger.LogError($"ItemType with id: {itemTypeDto.Id} not updated.");
            throw new Exception($"ItemType with id: {itemTypeDto.Id} not updated.");
        }

        return itemType.ToDto();
    }

    public async Task DeleteItemTypeAsync(Guid id, bool isSoftDeleting)
    {
        var result = await _itemTypeRepository.DeleteItemTypeAsync(id, isSoftDeleting);
        if (!result)
        {
            _logger.LogError($"ItemType with id: {id} not deleted.");
            throw new Exception($"ItemType with id: {id} not deleted.");
        }
    }
}
