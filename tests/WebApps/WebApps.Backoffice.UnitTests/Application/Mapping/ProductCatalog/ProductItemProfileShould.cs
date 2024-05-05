using AutoMapper;
using BackOffice.Application.Mapping.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Contracts.Dtos.ProductCatalog;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.ProductCatalog;

public class ProductItemProfileShould
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public ProductItemProfileShould()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductItemProfile>();
            cfg.AddProfile<ProductTypeProfile>();  
            cfg.AddProfile<ProductBrandProfile>();
        });

        _mapper = new Mapper(_configuration);
    }

    [Fact]
    public void HaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void MapProductItemToProductItemDto()
    {
        var productItem = new ProductItem
        {
            Id = Guid.NewGuid(),
            Description = "Test Description",
            Price = 10.0m,
            PictureUri = "http://test.com",
            PictureBase64 = "base64",
            Barcode = "1234567890",
            CodeNumber = 1,
            CodePrefix = "Test",
            Name = "Test Name",
            ParentId = Guid.NewGuid(),
            ProductBrand = new ProductBrand
            {
                Id = Guid.NewGuid(),
                Name = "Test Brand"
            },
            ProductType = new ProductType
            {
                Id = Guid.NewGuid(),
                Name = "Test Type"
            }
        };

        var productItemDto = _mapper.Map<ProductItemDto>(productItem);

        productItemDto.Should().NotBeNull();
        productItemDto.Id.Should().Be(productItem.Id);
        productItemDto.Description.Should().Be(productItem.Description);
        productItemDto.Price.Should().Be(productItem.Price);
        productItemDto.PictureUri.Should().Be(productItem.PictureUri);
        productItemDto.PictureBase64.Should().Be(productItem.PictureBase64);
        productItemDto.Barcode.Should().Be(productItem.Barcode);
        productItemDto.CodeNumber.Should().Be(productItem.CodeNumber);
        productItemDto.CodePrefix.Should().Be(productItem.CodePrefix);
        productItemDto.Name.Should().Be(productItem.Name);
        productItemDto.ParentId.Should().Be(productItem.ParentId);
        productItemDto.ProductBrand.Should().NotBeNull();
        productItemDto.ProductBrand?.Id.Should().Be(productItem.ProductBrand.Id);
        productItemDto.ProductBrand?.Name.Should().Be(productItem.ProductBrand.Name);
        productItemDto.ProductType.Should().NotBeNull();
        productItemDto.ProductType?.Id.Should().Be(productItem.ProductType.Id);
        productItemDto.ProductType?.Name.Should().Be(productItem.ProductType.Name);
    }
}
