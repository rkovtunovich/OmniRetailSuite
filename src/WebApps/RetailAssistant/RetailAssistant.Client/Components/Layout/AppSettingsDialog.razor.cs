using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace RetailAssistant.Client.Components.Layout;

public partial class AppSettingsDialog
{
    [Inject] private IUserPreferenceService _userPreferenceService { get; set; } = default!;

    [Inject] private AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    [Inject] private NavigationManager _navigationManager { get; set; } = default!;

    [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

    [Inject] private IStringLocalizer<AppSettingsDialog> _localizer { get; set; } = default!;

    [CascadingParameter] private MudDialogInstance AppSettingsDialogInstance { get; set; } = default!;

    [Parameter] public EventCallback<bool>? OnThemeChanged { get; set; }

    private Settings _settings = new();

    private List<Language> _languages = [Language.EN, Language.UK];

    private bool _isLanguageChanged;

    private bool _isDarkMode;

    protected override async Task OnInitializedAsync()
    {
        var userId = await _authenticationStateProvider.GetUserId();
        if (userId is null)
            return;

        _settings = await _userPreferenceService.GetPreferencesAsync(userId) ?? new();
        _isDarkMode = _settings.Theme == UITheme.Dark;
    }

    private void ChangeTheme()
    {
        _isDarkMode = !_isDarkMode;
        _settings.Theme = _isDarkMode ? UITheme.Dark : UITheme.Light;

        OnThemeChanged?.InvokeAsync(_isDarkMode);
    }

    private void ChangeLanguage(Language language)
    {
        _isLanguageChanged = true;

        _settings.Language = language;
    }

    private async Task SaveSettings()
    {
        var userId = await _authenticationStateProvider.GetUserId();
        if (userId is null)
            return;

        await _userPreferenceService.UpdatePreferencesAsync(userId, _settings);

        AppSettingsDialogInstance.Close(DialogResult.Ok(_settings));

        if (_isLanguageChanged)
        {
            var cultureString = _settings.Language.ToString().ToLower();
            await _jsRuntime.InvokeVoidAsync("blazorCulture.set", cultureString);
            _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
        }           
    }

    private void CloseSettings()
    {
        AppSettingsDialogInstance.Close(DialogResult.Ok(_settings));
    }
}
