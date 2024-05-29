using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.ProductCatalog.Parent;

public partial class ItemParentDetails : DetailsFormBase<ProductParent>
{
    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    protected override async Task<ProductParent> GetModel()
    {
        return await ProductParentService.GetByIdAsync(Id) ?? new();
    }

    private IList<ProductParent> _allIProductParents = [];

    private List<ProductParentSelectModel> _flattenedParents = [];

    private ProductParentSelectModel? _selectedParent;

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _flattenedParents = await GetFlattenedParentsAsync();
            CallRequestRefresh();
        }
    }

    #endregion

    #region Commands

    protected override void DefineFormToolbarCommands()
    {
        ToolbarCommands =
        [
            new()
            {
                Name = "Save",
                Icon = Icons.Material.Outlined.Save,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, SaveClick),
                Tooltip = "Save"
            },
            new()
            {
                Name = "Delete",
                Icon = Icons.Material.Outlined.Delete,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, DeleteClick),
                Tooltip = "Delete"
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

    private async Task SaveClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        Model.ParentId = _selectedParent?.Id;

        var result = await ProductParentService.UpdateAsync(Model);
        if (result)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task DeleteClick()
    {
        await ProductParentService.DeleteAsync(Model.Id, true);

        CloseClick();
    }

    #endregion

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

    private Func<ProductParentSelectModel, string> _converterProductParent = p => p?.Name ?? "";

}
