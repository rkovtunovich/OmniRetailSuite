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

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<Receipt> eventArg)
    {
        SelectedItem = eventArg.Item;
        DefineContextMenuItems();

        await ContextMenu.Show(eventArg.MouseEventArgs);
    }

    private void DefineContextMenuItems()
    {
        ContextMenuItems =
        [
            new () {
                Text = _localizer["Open"],
                Icon = Icons.Material.Outlined.OpenInNew,
                OnClick = EventCallback.Factory.Create(this, () => OpenItem<ReceiptDetails>(SelectedItem?.Id))
            }
        ];
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
