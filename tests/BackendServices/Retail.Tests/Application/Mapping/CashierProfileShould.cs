using Retail.Application.Mapping;

namespace Retail.Tests.Application.Mapping;
public class CashierProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CashierProfile>();
        });

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
