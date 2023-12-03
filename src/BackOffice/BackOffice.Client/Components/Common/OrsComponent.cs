namespace BackOffice.Client.Components.Common;

public abstract class OrsComponent : ComponentBase, IDisposable
{
    private readonly RefreshBroadcast _refresh = RefreshBroadcast.Instance;

    protected override void OnInitialized()
    {
        _refresh.RefreshRequested += DoRefresh;
        base.OnInitialized();
    }

    public void CallRequestRefresh()
    {
        _refresh.CallRequestRefresh();
    }

    private async void DoRefresh()
    {
        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }

    public virtual void Dispose()
    {

    }
}
