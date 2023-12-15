using BackOffice.Application.Services.Abstraction.Retail;
using BackOffice.Client.Components.Base;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using BackOffice.Core.Models.Retail;

namespace BackOffice.Client.Pages.Retail.Cashiers;

public partial class CashierList : ListBase<Cashier>
{
    [Inject] public IRetailService<Cashier> RetailService { get; set; } = null!;

    private DataGridEditMode _editMode = DataGridEditMode.Form;
    private DataGridEditTrigger _editTrigger = DataGridEditTrigger.Manual;
    private DialogOptions _dialogOptions = new() { DisableBackdropClick = true };

    private Func<Cashier, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;

        if (x.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

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
        TabsService.TryCreateTab<CashierCreate>();
    }

    private void OpenClick(CellContext<Cashier> context)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(CashierDetails.Model), context.Item }
        };

        TabsService.TryCreateTab<CashierDetails>(parameters);
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
    }

    private async Task CommittedItemChanges(Cashier cashier)
    {
        await RetailService.UpdateAsync(cashier);

        CallRequestRefresh();
    }

    private async Task RowClick(DataGridRowClickEventArgs<Cashier> eventArg)
    {
        if (eventArg.MouseEventArgs.Detail == 1)
            return;

        _editMode = DataGridEditMode.Form;
        _editTrigger = DataGridEditTrigger.OnRowClick;

        await Task.Run(() =>
        {
        });
    }

    private async Task OnChanged(Cashier changed)
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
