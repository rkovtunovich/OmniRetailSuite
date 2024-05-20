using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using RetailAssistant.Core.Models;
using RetailAssistant.Core.Models.ProductCatalog;
using RetailAssistant.Core.Models.Retail;

namespace WebApps.RetailAssistant.UnitTests.Pages.CashiersWorkplace;

public class CashiersMainShould : TestContext
{
    private readonly IProductCatalogService<ProductParent> _productParentService;
    private readonly IProductCatalogService<CatalogProductItem> _productItemService;
    private readonly IRetailService<Receipt> _receiptService;
    private readonly ILocalConfigService _localConfigService;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IDialogService _dialogService;

    public CashiersMainShould()
    {
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);

        _productParentService = Substitute.For<IProductCatalogService<ProductParent>>();
        _productItemService = Substitute.For<IProductCatalogService<CatalogProductItem>>();
        _receiptService = Substitute.For<IRetailService<Receipt>>();
        _localConfigService = Substitute.For<ILocalConfigService>();
        _guidGenerator = Substitute.For<IGuidGenerator>();
        _dialogService = Substitute.For<IDialogService>();

        Services.AddMudServices();
        Services.AddSingleton(_productParentService);
        Services.AddSingleton(_productItemService);
        Services.AddSingleton(_receiptService);
        Services.AddSingleton(_localConfigService);
        Services.AddSingleton(_guidGenerator);
        Services.AddSingleton(_dialogService);
    }

    [Fact]
    public void SaveReceipt_ButtonClicked_ReceiptTableEmpty()
    {
        // Arrange
        _productParentService.GetAllAsync().Returns([]);
        _productItemService.GetAllAsync().Returns([new() { Id = Guid.NewGuid() }]);
        _localConfigService.GetConfigAsync().Returns(new RetailAssistantAppConfig()
        {
            Store = new Store()
        });

        var component = RenderComponent<CashiersMain>(parameters => parameters
            .Add(p => p.Cashier, new Cashier())
        );
        component.Instance.AddProductItemToReceipt(new() { Id = Guid.NewGuid() });
        component.Render();

        // Act
        var button = component.Find(".cashiers-receipt-save-button");
        button.Click();

        // Assert
        var rows = component.FindAll(".cashiers-workplace-receipt-table-row");
        rows.Should().BeEmpty();
    }

    [Fact]
    public void AddProductItemToReceipt_ProductItemAddedToReceipt_ReceiptTableHasOneRow()
    {
        // Arrange
        _productParentService.GetAllAsync().Returns([]);
        _productItemService.GetAllAsync().Returns([new() { Id = Guid.NewGuid() }]);
        _localConfigService.GetConfigAsync().Returns(new RetailAssistantAppConfig()
        {
            Store = new Store()
        });

        var component = RenderComponent<CashiersMain>(parameters => parameters
            .Add(p => p.Cashier, new Cashier())
        );

        // Act
        component.Instance.AddProductItemToReceipt(new() { Id = Guid.NewGuid() });
        component.Render();

        // Assert
        var rows = component.FindAll(".cashiers-workplace-receipt-table-row");
        rows.Should().HaveCount(1);
    }
}
