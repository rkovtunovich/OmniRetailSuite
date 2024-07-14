using AutoMapper;
using BackOffice.Application.Mapping.Retail;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.Retail;

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

        // Assert & Act
        configuration.AssertConfigurationIsValid();
    }
}
