using AutoMapper;
using BackOffice.Application.Mapping.Retail;

namespace WebApps.Backoffice.UnitTests.Application.Mapping.Retail;

public class ReceiptItemProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
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
