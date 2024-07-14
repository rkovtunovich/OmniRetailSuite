using AutoMapper;
using ProductCatalog.Application.Mapping;

namespace ProductCatalog.Tests.Application.Mapping;

public class ProductTypeProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductTypeProfile>());
        
        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
