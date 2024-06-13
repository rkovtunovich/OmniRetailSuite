namespace BackOffice.Client.Components.Pages.Retail.Cashiers;

public partial class CashierSelectionList : SelectionListBase<Cashier>
{
    [Inject] IDialogService DialogService { get; set; } = null!;

    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private async Task OnOkButtonClick()
    {
        await OnMultipleSelectionMade.InvokeAsync([.. SelectedItems]);
        
        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void OnCancelButtonClick()
    {
        MudDialog?.Cancel();
    }
}
