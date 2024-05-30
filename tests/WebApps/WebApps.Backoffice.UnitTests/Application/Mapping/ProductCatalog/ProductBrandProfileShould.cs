using AutoMapper;
using BackOffice.Application.Mapping.ProductCatalog;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.ProductCatalog;

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
