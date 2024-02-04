namespace RetailAssistant.Client.Pages.CashiersWorkplace;

public partial class CashiersWorkplace
{
    private Receipt _receipt = null!;

    private double _splitterPercentage = 75;

    protected override void OnInitialized()
    {
        _receipt = new Receipt();

        base.OnInitialized();
    }
}
