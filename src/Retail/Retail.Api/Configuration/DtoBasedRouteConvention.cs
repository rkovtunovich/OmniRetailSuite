using Contracts.Dtos.Retail;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Retail.Api.Controllers;

namespace Retail.Api.Configuration;

public class DtoBasedRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.Selectors.Count is 0)
            return;

        var routeModel = controller.Selectors[0].AttributeRouteModel;
        if (routeModel is null)
            return;

        var template = routeModel.Template;
        if (template is null)
            return;

        var resourceName = GetResourceNameByDto(controller.ControllerType.Name);
        var newTemplate = template.Replace("{resource}", resourceName);
        routeModel.Template = newTemplate;
    }

    private string GetResourceNameByDto(string controllerName) => controllerName switch
    {
        nameof(CashierController) => nameof(CashierDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        nameof(ProductItemController) => nameof(ProductItemDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        nameof(ReceiptController) => nameof(ReceiptDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        nameof(StoreController) => nameof(StoreDto).TrimEnd("Dto".ToCharArray()).ToLower() + "s",
        _ => throw new ArgumentException("Controller not found")
    };
}
