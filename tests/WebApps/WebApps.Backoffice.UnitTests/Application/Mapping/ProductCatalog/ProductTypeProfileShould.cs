using AutoMapper;
using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Contracts.Dtos.ProductCatalog;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.ProductCatalog;

public class ProductTypeProfileShould
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public ProductTypeProfileShould()
    {
        _configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductTypeProfile>());
        _mapper = new Mapper(_configuration);
    }

    [Fact]
    public void HaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void MapProductTypeToProductTypeDto()
    {
        var productType = new ProductType
        {
            Id = Guid.NewGuid(),
            Name = "Test Name"
        };

        var productTypeDto = _mapper.Map<ProductTypeDto>(productType);

        productTypeDto.Should().NotBeNull();
        productTypeDto.Id.Should().Be(productType.Id);
        productTypeDto.Name.Should().Be(productType.Name);
    }
}
