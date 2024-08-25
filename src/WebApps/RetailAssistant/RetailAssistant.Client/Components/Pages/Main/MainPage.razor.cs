namespace RetailAssistant.Client.Components.Pages.Main;

public partial class MainPage
{
    [Inject] private IStringLocalizer<MainPage> _localizer { get; set; } = null!;

    [Inject] private ILocalConfigService LocalConfigService { get; set; } = null!;

    private Store? _currentStore;

    private bool _isStoreSelected;

    protected override async Task OnInitializedAsync()
    {
        var localSettings = await LocalConfigService.GetConfigAsync();

        _currentStore = localSettings.Store;
        _isStoreSelected = _currentStore is not null;
    }
}
