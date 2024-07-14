namespace BackOffice.Client.Localization;

public class BackofficeLocalizer(IStringLocalizer<BackofficeLocalizer> localizer) : MudLocalizer
{
    public override LocalizedString this[string key] => localizer[key];
}
