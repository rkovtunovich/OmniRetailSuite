using AutoMapper;
using ProductCatalog.Application.Mapping;

namespace ProductCatalog.Tests.Application.Mapping;

public class ProductItemProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductItemProfile>();
            cfg.AddProfile<ProductTypeProfile>();
            cfg.AddProfile<ProductBrandProfile>();
        });

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
