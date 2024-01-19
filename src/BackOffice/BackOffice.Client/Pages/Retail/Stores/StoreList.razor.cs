namespace BackOffice.Client.Pages.Retail.Stores;

public partial class StoreList : ListBase<Store>
{
    [Inject] public IRetailService<Store> RetailService { get; set; } = null!;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            RetailService.OnChanged += OnChanged;

            await ReloadItems();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void CreateClick()
    {
        TabsService.TryCreateTab<StoreCreate>();
    }

    private void OpenClick(CellContext<Store> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(StoreDetails.Id), context.Item.Id }
        };

        TabsService.TryCreateTab<StoreDetails>(parameters);
    }

    private async Task ReloadItems()
    {
        Items = await RetailService.GetAllAsync();

        CallRequestRefresh();
    }

    private void StartedEditingItem(Store item)
    {
        _editTrigger = DataGridEditTrigger.Manual;
    }

    private void CanceledEditingItem(Store item)
    {
    }

    private async Task CommittedItemChanges(Store cashier)
    {
        await RetailService.UpdateAsync(cashier);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<Store> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.CompletedTask;
    }

    private async Task OnChanged(Store changed)
    {
        Items = await RetailService.GetAllAsync();
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
