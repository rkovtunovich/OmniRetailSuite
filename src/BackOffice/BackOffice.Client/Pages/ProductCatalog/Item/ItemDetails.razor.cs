﻿using BackOffice.Application.Services.Abstraction.ProductCatalog;
using BackOffice.Client.Model;
using BackOffice.Client.Services;
using BackOffice.Core.Models.ProductCatalog;
using Microsoft.AspNetCore.Components.Forms;

namespace BackOffice.Client.Pages.ProductCatalog.Item;

public partial class ItemDetails
{
    #region Injects

    [Inject] private TabsService _tabsService { get; set; } = null!;

    [Inject] public IProductItemService ProductItemService { get; set; } = null!;

    [Inject] public IProductTypeService ProductTypeService { get; set; } = null!;

    [Inject] public IProductBrandService ProductBrandService { get; set; } = null!;

    [Inject] public IProductParentService ProductParentService { get; set; } = null!;

    #endregion

    #region Parameters

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    [Parameter]
    public ProductItem ProductItem { get; set; } = null!;

    #endregion

    #region Fields

    private List<ProductType> _itemTypes = [];

    private List<ProductBrand> _itemBrands = [];

    private List<ProductParentSelectModel> _flattenedParents = [];

    private ProductParentSelectModel? _selectedParent;

    private string LoadPicture => string.IsNullOrEmpty(ProductItem.PictureBase64) ? string.Empty : $"data:image/png;base64, {ProductItem.PictureBase64}";

    private bool HasPicture => !string.IsNullOrEmpty(ProductItem.PictureBase64);

    private string _badFileMessage = string.Empty;

    private EditContext? _editContext;

    #endregion

    #region Overrides

    protected override void OnInitialized()
    {
        _editContext = new EditContext(ProductItem);
    }

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

    #region Clicks

    private async Task SaveClick()
    {
        if (!_editContext?.Validate() ?? false)
            return;

        if (_selectedParent is not null)       
            ProductItem.ParentId = _selectedParent.Id;
        

        var result = await ProductItemService.UpdateItemAsync(ProductItem);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task DeleteClick()
    {
        await ProductItemService.DeleteItemAsync(ProductItem.Id, true);

        CloseClick();
    }

    private void CloseClick()
    {
        _tabsService.RemoveTab(_tabsService.Tabs?.ActivePanel);
    }

    #endregion

    private async Task<List<ProductParentSelectModel>> GetFlattenedParentsAsync()
    {
        var allParents = await ProductParentService.GetItemParentsAsync();
        var flattenedList = new List<ProductParentSelectModel>();
        FlattenTree(allParents, flattenedList, 0);

        if (ProductItem.ParentId is not null)
            _selectedParent = flattenedList.FirstOrDefault(p => p.Id == ProductItem.ParentId);

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

    #region Delegates

    private Func<ProductType, string> _converterItemType = p => p?.Name ?? "";

    private Func<ProductBrand, string> _converterItemBrand = p => p?.Name ?? "";

    private Func<ProductParentSelectModel, string> _converterItemParent = p => p?.Name ?? "";

    #endregion
}
