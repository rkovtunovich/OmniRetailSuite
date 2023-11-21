using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Parent;

public partial class ItemParentCreate
{
    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductParentService ProductParentService { get; set; } = null!;

    [Inject] public ILogger<ItemParentCreate> Logger { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private List<ProductParent> _allItemParents = [];
    private ProductParent _itemParent = new();

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(_itemParent);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadAllItemParents();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task CreateClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        var result = await ProductParentService.CreateItemParentAsync(_itemParent);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            Close();
        }
    }

    private void Close()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }

    private async Task LoadAllItemParents()
    {
        try
        {
            _allItemParents = await ProductParentService.GetItemParentsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error loading item parents: {ex}");
        }
    }

    private async Task OnParentSelected(Guid? parentId)
    {
        _itemParent.ParentId = parentId;
        if (parentId.HasValue)
            _itemParent.Parent = await ProductParentService.GetItemParentByIdAsync(parentId.Value);
        else
            _itemParent.Parent = null;
    }
}
