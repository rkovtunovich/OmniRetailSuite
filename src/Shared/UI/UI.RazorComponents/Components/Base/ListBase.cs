using UI.Razor.Components.Common;

namespace UI.Razor.Components.Base;

public abstract class ListBase<TItem> : OrsComponentBase where TItem : class
{
    [Inject] protected TabsService TabsService { get; set; } = null!;

    [Parameter]
    public List<ToolbarCommand> ToolbarCommands { get; set; } = null!;

    [Parameter]
    public IList<TItem> Items { get; set; } = [];

    protected TItem? SelectedItem { get; set; }

    protected string? QuickFilterSearchString { get; set; }

    protected Func<TItem, bool> QuickFilter { get; set; } = null!;

    protected ContextMenu ContextMenu { get; set; } = null!;

    protected List<ContextMenuItem> ContextMenuItems { get; set; } = [];

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

    protected virtual string SelectedRowClassFunc(TItem currentItem, int line)
    {
        if (SelectedItem is null)
            return string.Empty;

        if (SelectedItem == currentItem)
            return CssClassNamesHelper.SelectedRow;

        return string.Empty;
    }

    private void SetDefaultQuickFilter()
    {
        QuickFilter = x =>
        {
            if (string.IsNullOrWhiteSpace(QuickFilterSearchString))
                return true;

            var properties = typeof(TItem).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(x);

                if (value is null)
                    continue;

                if (value.ToString()?.Contains(QuickFilterSearchString, StringComparison.OrdinalIgnoreCase) ?? false)
                    return true;
            }

            return false;
        };
    }
}
