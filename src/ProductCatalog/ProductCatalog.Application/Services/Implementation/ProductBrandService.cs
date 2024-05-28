using Microsoft.Extensions.Logging;

namespace ProductCatalog.Application.Services.Implementation;

public class ProductBrandService(IBrandRepository brandRepository, IMapper mapper, ILogger<ProductBrandService> logger) : IBrandService
{
    public async Task<ProductBrandDto> GetBrandByIdAsync(Guid id)
    {
        var brand = await brandRepository.GetBrandByIdAsync(id);
        if (brand is null)
        {
            logger.LogError($"Brand with id: {id} not found.");
            throw new Exception($"Brand with id: {id} not found.");
        }

        return mapper.Map<ProductBrandDto>(brand);
    }

    public async Task<List<ProductBrandDto>> GetBrandsAsync()
    {
        var brands = await brandRepository.GetBrandsAsync();
        return mapper.Map<List<ProductBrandDto>>(brands);
    }

    public async Task<ProductBrandDto> CreateBrandAsync(ProductBrandDto brandDto)
    {
        var brand = mapper.Map<ProductBrand>(brandDto);
        var result = await brandRepository.CreateBrandAsync(brand);
        if (!result)
        {
            logger.LogError($"Brand with name: {brandDto.Name} not created.");
            throw new Exception($"Brand with name: {brandDto.Name} not created.");
        }

        return mapper.Map<ProductBrandDto>(brand);
    }

    public async Task<ProductBrandDto> UpdateBrandAsync(ProductBrandDto brandDto)
    {
        var brand = mapper.Map<ProductBrand>(brandDto);
        var result = await brandRepository.UpdateBrandAsync(brand);
        if (!result)
        {
            logger.LogError($"Brand with id: {brandDto.Id} not updated.");
            throw new Exception($"Brand with id: {brandDto.Id} not updated.");
        }

        return mapper.Map<ProductBrandDto>(brand);
    }

    public async Task DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        var result = await brandRepository.DeleteBrandAsync(id, useSoftDeleting);
        if (!result)
        {
            logger.LogError($"Brand with id: {id} not deleted.");
            throw new Exception($"Brand with id: {id} not deleted.");
        }
    }
}
