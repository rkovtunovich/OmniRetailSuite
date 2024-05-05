using AutoMapper;
using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Contracts.Dtos.ProductCatalog;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.ProductCatalog;

public class ProductBrandProfileShould
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public ProductBrandProfileShould()
    {
        _configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductBrandProfile>());
        _mapper = new Mapper(_configuration);
    }

    [Fact]
    public void HaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void MapProductBrandToProductBrandDto()
    {
        var productBrand = new ProductBrand
        {
            Id = Guid.NewGuid(),
            Name = "Test Name"
        };

        var productBrandDto = _mapper.Map<ProductBrandDto>(productBrand);

        productBrandDto.Should().NotBeNull();
        productBrandDto.Id.Should().Be(productBrand.Id);
        productBrandDto.Name.Should().Be(productBrand.Name);
    }
}
