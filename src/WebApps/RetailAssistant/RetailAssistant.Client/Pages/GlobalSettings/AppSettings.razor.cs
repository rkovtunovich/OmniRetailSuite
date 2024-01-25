using UI.Razor.Components.Base;

namespace RetailAssistant.Client.Pages.GlobalSettings;

public partial class AppSettings: OrsComponentBase
{
    [Inject]
    private ILocalConfigService LocalConfigService { get; set; } = null!;

    [Inject]
    private IRetailService<Store> _retailService { get; set; } = null!;

    private RetailAssistantAppConfig _appConfig = new();

    private List<Store> _stores = [];

    protected override async Task OnInitializedAsync()
    {
        _appConfig = await LocalConfigService.GetConfigAsync();
        _stores = await _retailService.GetAllAsync();

         await base.OnInitializedAsync();
    }

    private async Task Save()
    {
        await LocalConfigService.SaveConfigAsync(_appConfig);
    }
}
