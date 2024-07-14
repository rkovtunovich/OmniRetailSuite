namespace UI.Razor.Services.Abstraction;

public interface ITabsService
{
    public event Action? OnTabChanged;

    public string CurrentTab { get; }

    public List<TabView> TabDescriptions { get; }

    public void TryCreateTab<TFragment>(Dictionary<string, object>? parameters = null) where TFragment : ComponentBase;
}
