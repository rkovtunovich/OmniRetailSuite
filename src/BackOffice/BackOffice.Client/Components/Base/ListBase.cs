using BackOffice.Client.Services;

namespace BackOffice.Client.Components.Base;

public abstract class ListBase<TItem> : OrsComponentBase where TItem : class
{
    [Inject] protected TabsService TabsService { get; set; } = null!;

    [Parameter]
    public List<ToolbarCommand> ToolbarCommands { get; set; } = null!;

    [Parameter]
    public List<TItem> Items { get; set; } = [];

    protected string? _quickFilterSearchString;

    protected Func<TItem, bool> _quickFilter = null!;

    protected virtual void DefineToolbarCommands()
    {

    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        DefineToolbarCommands();
        SetDefaultQuickFilter();      
    }

    protected virtual void CloseClick()
    {
        TabsService.RemoveTab(TabsService.Tabs?.ActivePanel);
    }

    private void SetDefaultQuickFilter()
    {
        _quickFilter = x =>
        {
            if (string.IsNullOrWhiteSpace(_quickFilterSearchString))
                return true;

            var properties = typeof(TItem).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(x);

                if (value is null)
                    continue;

                if (value.ToString()?.Contains(_quickFilterSearchString, StringComparison.OrdinalIgnoreCase) ?? false)
                    return true;
            }

            return false;
        };
    }
}
