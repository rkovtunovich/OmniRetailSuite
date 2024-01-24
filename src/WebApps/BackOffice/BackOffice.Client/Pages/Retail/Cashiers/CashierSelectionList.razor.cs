namespace BackOffice.Client.Pages.Retail.Cashiers;

public partial class CashierSelectionList : SelectionListBase<Cashier>
{
    [Inject] IDialogService DialogService { get; set; } = null!;

    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    private async Task OnOkButtonClick()
    {
        await OnSelectionMade.InvokeAsync(SelectedItems.ToList());
        
        MudDialog?.Close(DialogResult.Ok(true));
    }

    private void OnCancelButtonClick()
    {
        MudDialog?.Cancel();
    }
}
