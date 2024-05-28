using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ProductTypeService(IItemTypeRepository itemTypeRepository, IMapper mapper, ILogger<ProductTypeService> logger) : IItemTypeService
{
    public async Task<List<ProductTypeDto>> GetItemTypesAsync()
    {
        var itemTypes = await itemTypeRepository.GetItemTypesAsync();
        return mapper.Map<List<ProductTypeDto>>(itemTypes);
    }

    public async Task<ProductTypeDto> GetItemTypeByIdAsync(Guid id)
    {
        var itemType = await itemTypeRepository.GetItemTypeByIdAsync(id);
        if (itemType is null)
        {
            logger.LogError($"ItemType with id: {id} not found.");
            throw new Exception($"ItemType with id: {id} not found.");
        }

        return mapper.Map<ProductTypeDto>(itemType);
    }

    public async Task<ProductTypeDto> CreateItemTypeAsync(ProductTypeDto itemTypeDto)
    {
        var itemType = mapper.Map<ProductType>(itemTypeDto);
        var result = await itemTypeRepository.CreateItemTypeAsync(itemType);
        if (!result)
        {
            logger.LogError($"ItemType with name: {itemTypeDto.Name} not created.");
            throw new Exception($"ItemType with name: {itemTypeDto.Name} not created.");
        }

        return mapper.Map<ProductTypeDto>(itemType);
    }

    public async Task<ProductTypeDto> UpdateItemTypeAsync(ProductTypeDto itemTypeDto)
    {
        var itemType = mapper.Map<ProductType>(itemTypeDto);
        var result = await itemTypeRepository.UpdateItemTypeAsync(itemType);
        if (!result)
        {
            logger.LogError($"ItemType with id: {itemTypeDto.Id} not updated.");
            throw new Exception($"ItemType with id: {itemTypeDto.Id} not updated.");
        }

        return mapper.Map<ProductTypeDto>(itemType);
    }

    public async Task DeleteItemTypeAsync(Guid id, bool isSoftDeleting)
    {
        var result = await itemTypeRepository.DeleteItemTypeAsync(id, isSoftDeleting);
        if (!result)
        {
            logger.LogError($"ItemType with id: {id} not deleted.");
            throw new Exception($"ItemType with id: {id} not deleted.");
        }
    }
}
