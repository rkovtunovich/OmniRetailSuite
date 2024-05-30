using AutoMapper;
using BackOffice.Application.Mapping.Retail;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.Retail;

public class ReceiptProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ReceiptProfile>();
            cfg.AddProfile<ReceiptItemProfile>();
        });

        // Assert & Act
        configuration.AssertConfigurationIsValid();
    }
}
