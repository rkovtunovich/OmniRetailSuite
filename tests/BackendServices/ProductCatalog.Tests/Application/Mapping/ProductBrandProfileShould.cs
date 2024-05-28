using AutoMapper;
using ProductCatalog.Application.Mapping;

namespace ProductCatalog.Tests.Application.Mapping;

public class ProductBrandProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductBrandProfile>());
        
        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
