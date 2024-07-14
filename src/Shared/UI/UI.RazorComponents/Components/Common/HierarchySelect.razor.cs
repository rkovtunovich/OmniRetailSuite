namespace UI.Razor.Components.Common;

public partial class HierarchySelect : OrsComponentBase
{
    [Parameter]
    public List<HierarchySelectModel> Items { get; set; } = [];

    [Parameter]
    public Guid OwnerId { get; set; }

    [Parameter]
    public Guid? SelectedId { get; set; }

    [Parameter]
    public string Label { get; set; } = "Item";

    [Parameter]
    public EventCallback<HierarchySelectModel> OnChanged { get; set; }

    private HierarchySelectModel? _selectedItem;

    private List<HierarchySelectModel> _flattenedItems = [];

    private Func<HierarchySelectModel, string> _converterItem = p => p?.Name ?? "";

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        FlattenTree(Items, 0);

        if (SelectedId is not null)
            _selectedItem = _flattenedItems.FirstOrDefault(p => p.Id == SelectedId);
    }

    private void FlattenTree(List<HierarchySelectModel> items, int level)
    {
        foreach (var item in items)
        {
            var prefix = new string('-', level * 2); // 2 spaces per level for indentation
            _flattenedItems.Add(new HierarchySelectModel
            {
                Id = item.Id,
                Name = $"{prefix}{item.Name}"
            });

            if (item.Children is not null)
                FlattenTree(item.Children, level + 1);
        }
    }

    private void ChangeItem(HierarchySelectModel item)
    {
        _selectedItem = item;
        OnChanged.InvokeAsync(item);
    }
}

