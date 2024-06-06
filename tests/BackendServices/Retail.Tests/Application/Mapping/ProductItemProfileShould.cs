using Retail.Application.Mapping;

namespace Retail.Tests.Application.Mapping;

public class ProductItemProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ProductItemProfile>());

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
