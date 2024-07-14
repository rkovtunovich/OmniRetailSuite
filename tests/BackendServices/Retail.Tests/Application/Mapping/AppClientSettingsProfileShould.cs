using Retail.Application.Mapping;

namespace Retail.Tests.Application.Mapping;

public class AppClientSettingsProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {

            cfg.AddProfile<AppClientSettingsProfile>();
            cfg.AddProfile<StoreProfile>();
            cfg.AddProfile<CashierProfile>();
        });
    
        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
