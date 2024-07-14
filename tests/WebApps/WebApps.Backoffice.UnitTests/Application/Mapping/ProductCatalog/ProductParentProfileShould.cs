using AutoMapper;
using BackOffice.Application.Mapping.ProductCatalog;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.ProductCatalog;

public class ProductParentProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductParentProfile>();
        });

        // Assert & Act
        configuration.AssertConfigurationIsValid();
    }
}
