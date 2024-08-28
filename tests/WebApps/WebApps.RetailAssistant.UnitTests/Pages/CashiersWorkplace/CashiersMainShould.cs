using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor.Services;
using RetailAssistant.Core.Models;
using RetailAssistant.Core.Models.ProductCatalog;
using RetailAssistant.Core.Models.Retail;
using RetailAssistant.Data;

namespace WebApps.RetailAssistant.UnitTests.Pages.CashiersWorkplace;

public class CashiersMainShould : TestContext
{
    private readonly IApplicationRepository<ProductParent, ProductCatalogDbSchema> _productParentRepository;
    private readonly IApplicationRepository<CatalogProductItem, ProductCatalogDbSchema> _productItemRepository;
    private readonly IApplicationRepository<Receipt, RetailDbSchema> _receiptRepository;
    private readonly ILocalConfigService _localConfigService;
    private readonly IGuidGenerator _guidGenerator;

    public CashiersMainShould()
    {
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);

        _productParentRepository = Substitute.For<IApplicationRepository<ProductParent, ProductCatalogDbSchema>>();
        _productItemRepository = Substitute.For<IApplicationRepository<CatalogProductItem, ProductCatalogDbSchema>>();
        _receiptRepository = Substitute.For<IApplicationRepository<Receipt, RetailDbSchema>>();
        _localConfigService = Substitute.For<ILocalConfigService>();
        _guidGenerator = Substitute.For<IGuidGenerator>();

        Services.AddMudServices();
        Services.AddSingleton(_productParentRepository);
        Services.AddSingleton(_productItemRepository);
        Services.AddSingleton(_receiptRepository);
        Services.AddSingleton(_localConfigService);
        Services.AddSingleton(_guidGenerator);
        Services.AddSingleton(Substitute.For<IStringLocalizer<CashiersMain>>());
        Services.AddSingleton(Substitute.For<IStringLocalizer<ReceiptPayment>>());
    }

    [Fact]
    public void Payment_ButtonClicked_ReceiptTableEmpty()
    {
        // Arrange
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudElementRef.saveFocus", _ => true);
        JSInterop.SetupVoid("mudScrollManager.lockScroll", _ => true);
        _productParentRepository.GetAllAsync().Returns([]);
        _productItemRepository.GetAllAsync().Returns([new() { Id = Guid.NewGuid() }]);
        _localConfigService.GetConfigAsync().Returns(new RetailAssistantAppConfig()
        {
            Store = new Store()
        });

        var cashierMainComponent = RenderComponent<CashiersMain>(parameters => parameters
            .Add(p => p.Cashier, new Cashier())
        );

        cashierMainComponent.Instance.AddProductItemToReceipt(new()
        {
            Id = Guid.NewGuid(),
            Price = 1
        });

        cashierMainComponent.Render();

        // Act
        var button = cashierMainComponent.Find(".cashiers-receipt-save-button");
        button.Click();

        cashierMainComponent.WaitForState(() => cashierMainComponent.Instance.PaymentDialog is not null, TimeSpan.FromSeconds(10));

        var dialogService = Services.GetRequiredService<IDialogService>();
        var paymentDialog = cashierMainComponent.Instance.PaymentDialog;

        if (paymentDialog is not null)
        {
            var fragment = Render(paymentDialog?.RenderFragment ?? throw new InvalidOperationException());
            var receiptPaymentComponent = fragment.FindComponent<ReceiptPayment>();
            var payButton =  receiptPaymentComponent.Find("#receipt-payment-pay-button");
            payButton.Click();
        }

        // Wait for the ReceiptPaymentMade method to complete
        cashierMainComponent.WaitForState(() => cashierMainComponent.Instance.PaymentDialog is null, TimeSpan.FromSeconds(10));

        // Assert
        var rows = cashierMainComponent.FindAll(".cashiers-workplace-receipt-table-row");
        rows.Should().BeEmpty();
    }

    [Fact]
    public void AddProductItemToReceipt_ProductItemAddedToReceipt_ReceiptTableHasOneRow()
    {
        // Arrange
        _productParentRepository.GetAllAsync().Returns([]);
        _productItemRepository.GetAllAsync().Returns([new() { Id = Guid.NewGuid() }]);
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
        rows.Should().ContainSingle();
    }
}
