using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ItemParentService: IItemParentService
{
    private readonly IItemParentRepository _itemParentRepository;
    private readonly ILogger<ItemParentService> _logger;

    public ItemParentService(IItemParentRepository itemParentRepository, ILogger<ItemParentService> logger)
    {
        _itemParentRepository = itemParentRepository;
        _logger = logger;
    }

    public async Task<List<ProductParentDto>> GetItemParentsAsync()
    {
        var parents = await _itemParentRepository.GetItemParentsAsync();

        return parents.Select(x => x.ToDto()).ToList();
    }

    public async Task<ProductParentDto> GetItemParentByIdAsync(Guid id)
    {
        var parent = await _itemParentRepository.GetItemParentAsync(id);

        if (parent is null)
        {
            _logger.LogError($"ItemParent with id: {id} not found.");
            throw new Exception($"ItemParent with id: {id} not found.");
        }
            
        return parent.ToDto();
    }

    public async Task<ProductParentDto> CreateItemParentAsync(ProductParentDto itemParent)
    {
        var item = itemParent.ToEntity();

        var isCreated = await _itemParentRepository.CreateItemParentAsync(item);
        if (isCreated)
            return item.ToDto();

        _logger.LogError($"ItemParent with name: {itemParent.Name} not created.");
        throw new Exception($"ItemParent with name: {itemParent.Name} not created.");
    }

    public async Task<ProductParentDto> UpdateItemParentAsync(ProductParentDto itemParent)
    {
        var item = itemParent.ToEntity();

        var isUpdated = await _itemParentRepository.UpdateItemParentAsync(item);
        if (isUpdated)
            return item.ToDto();

        _logger.LogError($"ItemParent with id: {itemParent.Id} not updated.");
        throw new Exception($"ItemParent with id: {itemParent.Id} not updated.");
    }

    public async Task DeleteItemParentAsync(Guid id, bool isSoftDeleting)
    {
       var result = await _itemParentRepository.DeleteItemParentAsync(id, isSoftDeleting);

        if (!result)
        {
              _logger.LogError($"ItemParent with id: {id} not deleted.");
              throw new Exception($"ItemParent with id: {id} not deleted.");
         }
    }
}
