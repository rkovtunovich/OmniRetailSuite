using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Web;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemCreate: CreationFormBase<ProductItem>
{
    #region Injects

    [Inject] public IProductItemService ProductItemService { get; set; } = null!;

    [Inject] public IProductBrandService ProductBrandService { get; set; } = null!;

    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Inject] public IProductParentService ProductParentService { get; set; } = null!;

    #endregion

    #region Parameters

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    #endregion

    #region Fields

    private List<ProductType> _itemTypes = [];

    private List<ProductBrand> _itemBrands = [];

    private string LoadPicture => string.IsNullOrEmpty(Model.PictureBase64) ? string.Empty : $"data:image/png;base64, {Model.PictureBase64}";

    private bool HasPicture => !string.IsNullOrEmpty(Model.PictureBase64);

    private string _badFileMessage = string.Empty;

    private List<ProductParentSelectModel> _flattenedParents = [];

    private ProductParentSelectModel? _selectedParent;

    #endregion

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _flattenedParents = await GetFlattenedParentsAsync();
            _itemTypes = await ProductTypeService.GetTypesAsync();
            _itemBrands = await ProductBrandService.GetBrandsAsync();

            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    #region Private Methods

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

    #region Clicks

    private async Task CreateClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        if (_selectedParent is not null)
            Model.ParentId = _selectedParent.Id;

        var result = await ProductItemService.CreateItemAsync(Model);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    #endregion

    private async Task<List<ProductParentSelectModel>> GetFlattenedParentsAsync()
    {
        var allParents = await ProductParentService.GetItemParentsAsync();
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

    #endregion

    #region Delegate Handlers

    private Func<ProductType, string> _converterItemType = p => p?.Name ?? "";

    private Func<ProductBrand, string> _converterItemBrand = p => p?.Name ?? "";

    private Func<ProductParentSelectModel, string> _converterItemParent = p => p?.Name ?? "";

    #endregion
}
