using UI.Razor.Enums;

namespace UI.Razor.Components.Common;

public partial class SelectionTree<TItem> : OrsComponentBase where TItem : class, ITreeItem, new()
{
    private static readonly string selectedItemClassName = "ors-selected-parent-item";

    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = [];

    [Parameter]
    public EventCallback<TItem>? OnItemDoubleClick { get; set; }

    [Parameter]
    public EventCallback<TItem>? OnOpen { get; set; }

    private HashSet<TItem> _items = [];

    private TItem? _selectedItem = null;

    private void ReloadItems()
    {
        _items.Clear();
        _items.Add(new TItem { Id = Guid.NewGuid(), Name = FilterSpecialCase.All.ToString() });
        _items.Add(new TItem { Id = Guid.NewGuid(), Name = FilterSpecialCase.Empty.ToString() });

        foreach (var itemParent in Items)
        {
            _items.Add(itemParent);
        }

        CallRequestRefresh();
    }

    private void OpenItemClick(MudTreeViewItem<ITreeItem> viewItem)
    {
        if (viewItem.Value.Name == FilterSpecialCase.Empty.ToString())
            return;

        if (viewItem.Value.Name == FilterSpecialCase.All.ToString())
            return;

        OnOpen?.InvokeAsync(viewItem.Value as TItem);
    }

    private void OnItemDoubleClickHandler(ITreeItem item)
    {
        OnItemDoubleClick?.InvokeAsync(item as TItem);
    }

    private string GetItemClass(TItem item)
    {
        if (_selectedItem is null)
            return "";

        if (_selectedItem.Name == FilterSpecialCase.All.ToString() && item.Name == _selectedItem.Name)
            return selectedItemClassName;

        if (_selectedItem.Name == FilterSpecialCase.Empty.ToString() && item.Name == _selectedItem.Name)
            return selectedItemClassName;

        return _selectedItem.Id == item.Id ? selectedItemClassName : "";
    }
}
