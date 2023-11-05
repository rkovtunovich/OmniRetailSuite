using Retail.Core.Models;

namespace Retail.Client.Wasm.Pages.CashiersWorkpalce;

public partial class CashiersWorkplace
{
    private Receipt _receipt = null!;

    protected override void OnInitialized()
    {
        _receipt = new Receipt();

        base.OnInitialized();
    }
}
