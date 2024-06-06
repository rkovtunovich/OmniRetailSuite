using Retail.Application.Mapping;

namespace Retail.Tests.Application.Mapping;

public class ReceiptProfileShould
{
    [Fact]
    public void HaveValidConfiguration()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ReceiptProfile>();
            cfg.AddProfile<StoreProfile>();
            cfg.AddProfile<CashierProfile>();
            cfg.AddProfile<ReceiptItemProfile>();
            cfg.AddProfile<ProductItemProfile>();
        });

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }
}
