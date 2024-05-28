using AutoMapper;
using ProductCatalog.Application.Mapping;

namespace ProductCatalog.Tests.Application.Mapping;

public class ProductParentProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductParentProfile>());
        
        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
