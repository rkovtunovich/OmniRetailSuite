namespace BackOffice.Client.Components.Pages.Retail.ReceiptPages;

public partial class ReceiptList : ListBase<Receipt>
{
    [Inject] public IRetailService<Receipt> RetailService { get; set; } = null!;

    [Inject] private IStringLocalizer<ReceiptList> _localizer { get; set; } = default!;

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
