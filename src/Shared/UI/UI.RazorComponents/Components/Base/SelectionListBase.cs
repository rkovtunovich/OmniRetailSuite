namespace UI.Razor.Components.Base;

public class SelectionListBase<TItem> : ListBase<TItem> where TItem : class
{
    public HashSet<TItem> SelectedItems { get; set; } = [];

    [Parameter]
    public EventCallback<IEnumerable<TItem>> OnSelectionMade { get; set; }
}
