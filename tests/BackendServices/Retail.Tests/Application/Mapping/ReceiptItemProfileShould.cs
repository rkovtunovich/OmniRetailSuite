using Retail.Application.Mapping;

namespace Retail.Tests.Application.Mapping;

public class ReceiptItemProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ReceiptItemProfile>();
            cfg.AddProfile<ProductItemProfile>();
        });  

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
