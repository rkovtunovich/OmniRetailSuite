namespace RetailAssistant.Client.Pages.CashiersWorkplace;

public partial class CashiersWorkplace
{
    private Receipt _receipt = null!;

    protected override void OnInitialized()
    {
        _receipt = new Receipt();

        base.OnInitialized();
    }
}
