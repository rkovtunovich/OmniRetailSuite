using BackOffice.Client.Model.UI;
using Microsoft.IdentityModel.Tokens;

namespace BackOffice.Client.Services;

public class TabsService
{
    public event Action? OnTabChanged;
    
    public List<TabView> TabDescriptions { get; private set; } = new();

    public int TabIndex { get; set; } = 0;
    
    public int? NextTabIndex { get; private set; } = null;

    public MudTabs? Tabs { get; set; }

    public string CurrentTab { get; private set; } = "";

    public void ChangeTab(string tabKey)
    {
        CurrentTab = tabKey;
        OnTabChanged?.Invoke();
    }

    public void ActivateCurrentTab()
    {
        if(CurrentTab.IsNullOrEmpty())
            return;

        var tab = TabDescriptions.FirstOrDefault(x => x.Name == CurrentTab);
        // If tab already exists, activate it
        if (tab is null)
            return;

        Tabs?.ActivatePanel(tab.Id);

        CurrentTab = "";
    }

    public void RemoveTab(MudTabPanel? tabPanel)
    {
        if (tabPanel is null)
            return;

        var tab = TabDescriptions.FirstOrDefault(x => x.Id == (Guid)(tabPanel?.Tag ?? Guid.Empty));
        if (tab is null)
            return;

        TabDescriptions.Remove(tab);
        OnTabChanged?.Invoke();
    }

    public void TryCreateTab<TFragment>(Dictionary<string, object>? parameters = null) where TFragment : ComponentBase
    {
        if(Tabs is null)
            return;

        var tabName = typeof(TFragment).Name;

        var tab = TabDescriptions.FirstOrDefault(x => x.Name == tabName);
        // If tab already exists, activate it
        if (tab is not null)
        {
            Tabs.ActivatePanel(tab.Id);
            OnTabChanged?.Invoke();
            return;
        }
        
        var content = CreateFragment<TFragment>(parameters);

        var tabView = new TabView
        {
            Name = tabName,
            Content = content,
            Id = Guid.NewGuid()
        };

        CurrentTab = tabName;
        TabDescriptions.Add(tabView);
        NextTabIndex = TabDescriptions.Count - 1;
        OnTabChanged?.Invoke();

    }

    private RenderFragment CreateFragment<TFragment>(Dictionary<string, object>? parameters = null) where TFragment : ComponentBase => builder =>
    {
        builder.OpenComponent<TFragment>(0);
        if (parameters is not null)
        {
            foreach (var parameter in parameters)
            {
                builder.AddAttribute(1, parameter.Key, parameter.Value);
            }
        }
        builder.CloseComponent();
    };
}
