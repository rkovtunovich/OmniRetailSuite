namespace BackOffice.Client.Components.Pages.Retail.StorePages;

public partial class StoreList : ListBase<Store>
{
    [Inject] public IRetailService<Store> RetailService { get; set; } = null!;

    [Inject] private IStringLocalizer<StoreList> _localizer { get; set; } = default!;

    private DialogOptions _dialogOptions = new() { BackdropClick = false };

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            RetailService.OnChanged += OnChanged;

            await ReloadItems();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    private void DefineContextMenuItems()
    {
        ContextMenuItems =
        [
            new () {
                Text = _localizer["Open"],
                Icon = Icons.Material.Outlined.OpenInNew,
                OnClick = EventCallback.Factory.Create(this, () => OpenItem<StoreDetails>(SelectedItem?.Id))
            },
            new() {
                Text = _localizer["CreateByCopying"],
                Icon = Icons.Material.Outlined.Add,
                OnClick = EventCallback.Factory.Create(this, CreateNewItemBasedOnExisting)
            }
        ];
    }

    private void CreateClick()
    {
        TabsService.TryCreateTab<StoreCreate>();
    }

    private async Task ReloadItems()
    {
        Items = await RetailService.GetAllAsync();

        CallRequestRefresh();
    }

    private void RowClick(DataGridRowClickEventArgs<Store> eventArg)
    {
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        OpenItem<StoreDetails>(SelectedItem.Id);
    }

    private async Task OnChanged(Store changed)
    {
        Items = await RetailService.GetAllAsync();
        CallRequestRefresh();
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<Store> eventArg)
    {
        SelectedItem = eventArg.Item;
        DefineContextMenuItems();

        await ContextMenu.Show(eventArg.MouseEventArgs);
    }

    private void CreateNewItemBasedOnExisting()
    {
        if (SelectedItem is null)
            return;

        var clonedItem = SelectedItem.Clone();

        var parameters = new Dictionary<string, object>
        {
            { nameof(StoreCreate.Model), clonedItem }
        };

        TabsService.TryCreateTab<StoreCreate>(parameters);

        CallRequestRefresh();
    }

    public override void Dispose()
    {
        // Unsubscribe when the component is destroyed to prevent memory leaks
        RetailService.OnChanged -= OnChanged;
    }

    protected override void DefineToolbarCommands()
    {
        // empty
    }
}
