namespace BackOffice.Client.Components.Pages.Retail.CashierPages;

public partial class CashierList : ListBase<Cashier>
{
    [Inject] public IRetailService<Cashier> RetailService { get; set; } = null!;

    [Inject] private IStringLocalizer<CashierList> _localizer { get; set; } = default!;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

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
                OnClick = EventCallback.Factory.Create(this, () => OpenItem(SelectedItem))
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
        TabsService.TryCreateTab<CashierCreate>();
    }

    private void OpenItem(Cashier? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { nameof(Cashier.Id), item.Id }
        };

        TabsService.TryCreateTab<CashierDetails>(parameters);
    }

    private void OpenClick(CellContext<Cashier> context)
    {
        OpenItem(context.Item); 
    }

    private async Task ReloadItems()
    {
        Items = await RetailService.GetAllAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(Cashier item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(Cashier item)
    {
        // Do nothing
    }

    private async Task CommittedItemChanges(Cashier cashier)
    {
        await RetailService.UpdateAsync(cashier);

        CallRequestRefresh();
    }

    private void RowClick(DataGridRowClickEventArgs<Cashier> eventArg)
    {
        SelectedItem = eventArg.Item;

        if (eventArg.MouseEventArgs.Detail is 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;
    }

    private async Task OnChanged(Cashier changed)
    {
        Items = await RetailService.GetAllAsync();
        CallRequestRefresh();
    }

    private async Task RowClickContextMenu(DataGridRowClickEventArgs<Cashier> eventArg)
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
            { nameof(CashierCreate.Model), clonedItem }
        };

        TabsService.TryCreateTab<CashierCreate>(parameters);

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
