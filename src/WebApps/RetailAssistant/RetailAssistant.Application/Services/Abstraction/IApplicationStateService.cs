namespace RetailAssistant.Application.Services.Abstraction;

public interface IApplicationStateService
{
    public bool IsOnline { get; }

    public event Action? OnStateChange;
}
