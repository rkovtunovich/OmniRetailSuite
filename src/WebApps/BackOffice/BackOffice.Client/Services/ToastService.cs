using System.Timers;

namespace BackOffice.Client.Services;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error
}

public class ToastService : IDisposable
{
    public event Action<string, ToastLevel> OnShow = null!;
    public event Action OnHide = null!;

    private System.Timers.Timer _countdown = null!;

    public void ShowToast(string message, ToastLevel level)
    {
        OnShow?.Invoke(message, level);
        StartCountdown();
    }

    private void StartCountdown()
    {
        SetCountdown();
        if (_countdown.Enabled)
        {
            _countdown.Stop();
            _countdown.Start();
        }
        else
        {
            _countdown.Start();
        }
    }

    private void SetCountdown()
    {
        if (_countdown is null)
            return;

        _countdown = new System.Timers.Timer(3000);
        _countdown.Elapsed += HideToast;
        _countdown.AutoReset = false;
    }

    private void HideToast(object? source, ElapsedEventArgs args)
    {
        OnHide.Invoke();
    }

    public void Dispose()
    {
        _countdown?.Dispose();
    }
}
