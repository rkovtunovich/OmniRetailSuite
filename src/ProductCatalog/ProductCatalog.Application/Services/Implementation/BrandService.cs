using Microsoft.Extensions.Logging;


namespace ProductCatalog.Application.Services.Implementation;

public class BrandService: IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly ILogger<BrandService> _logger;

    public BrandService(IBrandRepository brandRepository, ILogger<BrandService> logger)
    {
        _brandRepository = brandRepository;
        _logger = logger;
    }

    public async Task<ProductBrandDto> GetBrandByIdAsync(Guid id)
    {
        var brand = await _brandRepository.GetBrandByIdAsync(id);
        if (brand is null)
        {
            _logger.LogError($"Brand with id: {id} not found.");
            throw new Exception($"Brand with id: {id} not found.");
        }

        return brand.ToDto();
    }

    public async Task<List<ProductBrandDto>> GetBrandsAsync()
    {
        var brands = await _brandRepository.GetBrandsAsync();
        return brands.Select(x => x.ToDto()).ToList();
    }

    public async Task<ProductBrandDto> CreateBrandAsync(ProductBrandDto brandDto)
    {
        var brand = brandDto.ToEntity();
        var result = await _brandRepository.CreateBrandAsync(brand);
        if (!result)
        {
            _logger.LogError($"Brand with name: {brandDto.Name} not created.");
            throw new Exception($"Brand with name: {brandDto.Name} not created.");
        }

        return brand.ToDto();
    }

    public async Task<ProductBrandDto> UpdateBrandAsync(ProductBrandDto brandDto)
    {
        var brand = brandDto.ToEntity();
        var result = await _brandRepository.UpdateBrandAsync(brand);
        if (!result)
        {
            _logger.LogError($"Brand with id: {brandDto.Id} not updated.");
            throw new Exception($"Brand with id: {brandDto.Id} not updated.");
        }

        return brand.ToDto();
    }

    public async Task DeleteBrandAsync(Guid id, bool useSoftDeleting)
    {
        var result = await _brandRepository.DeleteBrandAsync(id, useSoftDeleting);
        if (!result)
        {
            _logger.LogError($"Brand with id: {id} not deleted.");
            throw new Exception($"Brand with id: {id} not deleted.");
        }
    }
}
