namespace UI.Razor.Components.Base;

public class SelectionListBase<TItem> : ListBase<TItem> where TItem : class
{
    public HashSet<TItem> SelectedItems { get; set; } = [];

    public TItem? SelectedItem { get; set; }

    [Parameter]
    public EventCallback<IEnumerable<TItem>> OnMultipleSelectionMade { get; set; }

    [Parameter]
    public EventCallback<TItem> OnSingleSelectionMade { get; set; }
}
