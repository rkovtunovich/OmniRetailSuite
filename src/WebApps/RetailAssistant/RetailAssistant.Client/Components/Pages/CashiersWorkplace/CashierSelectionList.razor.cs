using Microsoft.Extensions.Localization;

namespace RetailAssistant.Client.Components.Pages.CashiersWorkplace;

public partial class CashierSelectionList : SelectionListBase<Cashier>
{
    [Inject] IDialogService DialogService { get; set; } = null!;

    [Inject] private IStringLocalizer<CashierSelectionList> _localizer { get; set; } = default!;

    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private async Task OnOkButtonClick()
    {
        await OnSingleSelectionMade.InvokeAsync(SelectedItem);

        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void OnCancelButtonClick()
    {
        MudDialog?.Cancel();
    }

    private void SelectedItemChanged(Cashier? cashier)
    {
        SelectedItem = cashier;
    }
}
