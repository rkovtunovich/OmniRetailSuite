using BackOffice.Client.Pages.Retail.Cashiers;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.Retail.Stores;

public partial class StoreDetails : DetailsFormBase<Store>
{
    [Inject] public IRetailService<Store> RetailService { get; set; } = null!;

    [Inject] public IRetailService<Cashier> CashierService { get; set; } = null!;

    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    #region Initialization

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _cashierTabToolbarCommands =
        [
            new()
            {
                Name = "Add",
                Icon = Icons.Material.Outlined.Add,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CashierTabOnAddClick),
                Tooltip = "Add"
            }
        ];
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _allCashiers = await GetAllCashiers();
    }

    protected override async Task<Store> GetModel()
    {
        return await RetailService.GetByIdAsync(Id) ?? new();
    }

    #endregion

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new()
            {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, SaveClick),
                Tooltip = "Save"
            },
            new()
            {
                Name = "Delete",
                Icon = Icons.Material.Outlined.Delete,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, DeleteClick),
                Tooltip = "Delete"
            },
            new()
            {
                Name = "Close",
                Icon = Icons.Material.Outlined.Close,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CloseClick),
                Tooltip = "Close"
            }
        ];
    }

    private async Task SaveClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await RetailService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task DeleteClick()
    {
        var result = await RetailService.DeleteAsync(Model.Id, true);

        if (result)
            CloseClick();
    }

    #endregion

    #region Cashiers Tab

    private List<ToolbarCommand> _cashierTabToolbarCommands = null!;

    private List<Cashier> _allCashiers = null!;

    private async Task CashierTabOnAddClick()
    {
        AddCashier();

        await Task.CompletedTask;
    }

    private void AddCashier()
    {
        var fragmentParameters = new Dictionary<string, object>
        {
            { nameof(CashierSelectionList.Items), _allCashiers },
            { nameof(CashierSelectionList.OnSelectionMade), EventCallback.Factory.Create<IEnumerable<Cashier>>(this, CashierTabOnSelectionChanged) }
        };

        var content = RenderFragmentBuilder.Create<CashierSelectionList>(fragmentParameters);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            CloseButton = true,
            
        };
        var dialogParameters = new DialogParameters { { "ChildContent", content } };

        var dialog = DialogService.Show<ModalComponent>("Select cashiers", dialogParameters, options);

    }

    private async Task<List<Cashier>> GetAllCashiers()
    {
        var cashiers = await CashierService.GetAllAsync();

        return cashiers;
    }

    private void CashierTabOnSelectionChanged(IEnumerable<Cashier> selectedItems)
    {
        Model.Cashiers = selectedItems.ToList();
    }

    #endregion
}
