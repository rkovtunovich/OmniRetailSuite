using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ProductCatalog.Api.Controllers;

namespace ProductCatalog.Api.Configuration;

public class DtoBasedRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if(controller.Selectors.Count is 0)
            return;

        var routeModel = controller.Selectors[0].AttributeRouteModel;
        if(routeModel is null)
            return;

        var template = routeModel.Template;
        if(template is null)
            return;
       
        var resourceName = GetResourceNameByDto(controller.ControllerType.Name);
        var newTemplate = template.Replace("{resource}", resourceName);
        routeModel.Template = newTemplate;
    }

    private string GetResourceNameByDto(string controllerName) => controllerName switch
    {
        nameof(BrandController) => nameof(ProductBrandDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        nameof(ItemTypeController) => nameof(ProductTypeDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        nameof(ItemController) => nameof(ProductItemDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        nameof(ItemParentController) => nameof(ProductParentDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        _ => throw new ArgumentException("Controller not found")
    };
}
