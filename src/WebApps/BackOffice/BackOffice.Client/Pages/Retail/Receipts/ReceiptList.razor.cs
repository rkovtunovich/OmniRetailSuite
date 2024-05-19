using BackOffice.Client.Pages.Retail.Stores;

namespace BackOffice.Client.Pages.Retail.Receipts;

public partial class ReceiptList : ListBase<Receipt>
{
    [Inject] public IRetailService<Receipt> RetailService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            RetailService.OnChanged += OnChanged;

            await ReloadItems();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void OpenClick(CellContext<Receipt> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(ReceiptDetails.Id), context.Item.Id }
        };

        TabsService.TryCreateTab<ReceiptDetails>(parameters);
    }

    private async Task ReloadItems()
    {
        Items = await RetailService.GetAllAsync();

        CallRequestRefresh();
    }

    private async Task OnChanged(Receipt changed)
    {
        Items = await RetailService.GetAllAsync();
        CallRequestRefresh();
    }
}
