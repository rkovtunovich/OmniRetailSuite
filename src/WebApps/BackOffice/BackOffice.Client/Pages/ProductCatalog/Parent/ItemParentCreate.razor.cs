using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.ProductCatalog.Parent;

public partial class ItemParentCreate: CreationFormBase<ProductParent>
{
    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public ILogger<ItemParentCreate> Logger { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private IList<ProductParent> _allIProductParents = [];

    private List<ProductParentSelectModel> _flattenedParents = [];

    private ProductParentSelectModel? _selectedParent;

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _flattenedParents = await GetFlattenedParentsAsync();
            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    private async Task CreateClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        if (_selectedParent is not null)
            Model.ParentId = _selectedParent.Id;

        var result = await ProductParentService.CreateAsync(Model);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task<List<ProductParentSelectModel>> GetFlattenedParentsAsync()
    {
        var allParents = await ProductParentService.GetAllAsync();
        var flattenedList = new List<ProductParentSelectModel>();
        FlattenTree(allParents, flattenedList, 0);

        if (Model.ParentId is not null)
            _selectedParent = flattenedList.FirstOrDefault(p => p.Id == Model.ParentId);

        return flattenedList;
    }

    private void FlattenTree(IEnumerable<ProductParent> parents, List<ProductParentSelectModel> list, int level)
    {
        foreach (var parent in parents)
        {
            var prefix = new string('-', level * 2); // 2 spaces per level for indentation
            list.Add(new ProductParentSelectModel
            {
                Id = parent.Id,
                Name = $"{prefix}{parent.Name}"
            });

            if (parent.Children is not null)
                FlattenTree(parent.Children, list, level + 1);
        }
    }

    private async Task OnParentSelected(Guid? parentId)
    {
        Model.ParentId = parentId;
        if (parentId.HasValue)
            Model.Parent = await ProductParentService.GetByIdAsync(parentId.Value);
        else
            Model.Parent = null;
    }

    private Func<ProductParentSelectModel, string> _converterProductParent = p => p?.Name ?? "";

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new()
            {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CreateClick),
                Tooltip = "Save"
            },
            new()
            {
                Name = "Close",
                Icon = Icons.Material.Outlined.Close,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CloseClick),
                Tooltip = "Close"
            }
        ];
    }

    #endregion
}
