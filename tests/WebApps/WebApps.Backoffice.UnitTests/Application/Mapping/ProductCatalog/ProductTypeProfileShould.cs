using AutoMapper;
using BackOffice.Application.Mapping.ProductCatalog;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.ProductCatalog;

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
