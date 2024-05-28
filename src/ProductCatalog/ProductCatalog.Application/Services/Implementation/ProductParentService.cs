using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ProductParentService(IItemParentRepository itemParentRepository, IMapper mapper, ILogger<ProductParentService> logger) : IItemParentService
{
    public async Task<List<ProductParentDto>> GetItemParentsAsync()
    {
        var parents = await itemParentRepository.GetItemParentsAsync();

        return mapper.Map<List<ProductParentDto>>(parents);
    }

    public async Task<ProductParentDto> GetItemParentByIdAsync(Guid id)
    {
        var parent = await itemParentRepository.GetItemParentAsync(id);

        if (parent is null)
        {
            logger.LogError($"ItemParent with id: {id} not found.");
            throw new Exception($"ItemParent with id: {id} not found.");
        }
            
        return mapper.Map<ProductParentDto>(parent);
    }

    public async Task<ProductParentDto> CreateItemParentAsync(ProductParentDto itemParent)
    {
        var item = mapper.Map<ProductParent>(itemParent);

        var isCreated = await itemParentRepository.CreateItemParentAsync(item);
        if (isCreated)
            return mapper.Map<ProductParentDto>(item);

        logger.LogError($"ItemParent with name: {itemParent.Name} not created.");
        throw new Exception($"ItemParent with name: {itemParent.Name} not created.");
    }

    public async Task<ProductParentDto> UpdateItemParentAsync(ProductParentDto itemParent)
    {
        var item = mapper.Map<ProductParent>(itemParent);

        var isUpdated = await itemParentRepository.UpdateItemParentAsync(item);
        if (isUpdated)
            return mapper.Map<ProductParentDto>(item);

        logger.LogError($"ItemParent with id: {itemParent.Id} not updated.");
        throw new Exception($"ItemParent with id: {itemParent.Id} not updated.");
    }

    public async Task DeleteItemParentAsync(Guid id, bool isSoftDeleting)
    {
       var result = await itemParentRepository.DeleteItemParentAsync(id, isSoftDeleting);

        if (!result)
        {
              logger.LogError($"ItemParent with id: {id} not deleted.");
              throw new Exception($"ItemParent with id: {id} not deleted.");
         }
    }
}
