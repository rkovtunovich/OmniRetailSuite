using AutoMapper;
using BackOffice.Core.Models.ProductCatalog;

namespace BackOffice.Client.Components.Pages.ProductCatalog.ParentPages;

public partial class ItemParentCreate : CreationFormBase<ProductParent>
{
    [Inject] public IProductCatalogService<ProductParent> ProductParentService { get; set; } = null!;

    [Inject] public ILogger<ItemParentCreate> Logger { get; set; } = null!;

    [Inject] public IMapper Mapper { get; set; } = null!;

    [Inject] private IStringLocalizer<ItemParentCreate> _localizer { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnSaveClick { get; set; }

    private IList<ProductParent> _allProductParents = [];

    #region Overrides

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _allProductParents = await ProductParentService.GetAllAsync();
            CallRequestRefresh();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #endregion

    private async Task CreateClick()
    {
        if (!EditContext?.Validate() ?? false)
            return;

        var result = await ProductParentService.CreateAsync(Model);
        if (result is not null)
        {
            await OnSaveClick.InvokeAsync(null);
            CloseClick();
        }
    }

    private async Task OnParentChanged(HierarchySelectModel selectModel)
    {
        Model.ParentId = selectModel?.Id;
        if (Model.ParentId is null || Model.ParentId == Guid.Empty)
            Model.Parent = null;
        else
            Model.Parent = await ProductParentService.GetByIdAsync(Model.ParentId.Value);

    }

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
                Tooltip = _localizer["Save"]
            },
            new()
            {
                Name = "Close",
                Icon = Icons.Material.Outlined.Close,
                Callback = EventCallback.Factory.Create<MouseEventArgs>(this, CloseClick),
                Tooltip = _localizer["Close"]
            }
        ];
    }

    #endregion
}
