using Retail.Application.Mapping;

namespace Retail.Tests.Application.Mapping;

public class StoreProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<StoreProfile>();
            cfg.AddProfile<CashierProfile>();
        });

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
