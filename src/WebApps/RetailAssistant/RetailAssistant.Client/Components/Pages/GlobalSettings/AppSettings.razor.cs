﻿namespace RetailAssistant.Client.Components.Pages.GlobalSettings;

public partial class AppSettings : OrsComponentBase
{
    [Inject]
    private ILocalConfigService LocalConfigService { get; set; } = null!;

    [Inject]
    private IRetailDataService<Store> _retailService { get; set; } = null!;

    private RetailAssistantAppConfig _appConfig = new();

    private IList<Store> _stores = [];

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _appConfig = await LocalConfigService.GetConfigAsync();
        _stores = await _retailService.GetAllAsync();
    }

    private async Task Save()
    {
        await LocalConfigService.SaveConfigAsync(_appConfig);
    }
}
