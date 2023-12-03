using BackOffice.Client.Services;

namespace BackOffice.Client.Components.Common;

public class ToastComponent : ComponentBase, IDisposable
{
    [Inject]
    ToastService ToastService { get; set; } = null!;

    protected string Heading { get; set; } = null!;

    protected string Message { get; set; } = null!;

    protected bool IsVisible { get; set; }

    protected string BackgroundCssClass { get; set; } = null!;

    protected string IconCssClass { get; set; } = null!;

    protected override void OnInitialized()
    {
        ToastService.OnShow += ShowToast;
        ToastService.OnHide += HideToast;
    }
    private async void ShowToast(string message, ToastLevel level)
    {
        BuildToastSettings(level, message);
        IsVisible = true;

        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }
    private async void HideToast()
    {
        IsVisible = false;

        await InvokeAsync(() =>
        {
            StateHasChanged();
        });
    }
    private void BuildToastSettings(ToastLevel level, string message)
    {
        switch (level)
        {
            case ToastLevel.Info:
                BackgroundCssClass = "bg-info";
                IconCssClass = "info";
                Heading = "Info";
                break;
            case ToastLevel.Success:
                BackgroundCssClass = "bg-success";
                IconCssClass = "check";
                Heading = "Success";
                break;
            case ToastLevel.Warning:
                BackgroundCssClass = "bg-warning";
                IconCssClass = "exclamation";
                Heading = "Warning";
                break;
            case ToastLevel.Error:
                BackgroundCssClass = "bg-danger";
                IconCssClass = "times";
                Heading = "Error";
                break;
        }
        Message = message;
    }
    public void Dispose()
    {
        ToastService.OnShow -= ShowToast;
    }
}
