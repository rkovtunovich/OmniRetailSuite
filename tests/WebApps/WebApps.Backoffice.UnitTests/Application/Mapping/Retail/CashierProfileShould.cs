using AutoMapper;
using BackOffice.Application.Mapping.Retail;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.Retail;

public class CashierProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<CashierProfile>());
        
        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
