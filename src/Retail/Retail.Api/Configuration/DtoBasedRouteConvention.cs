using Contracts.Dtos.Retail;
using Infrastructure.Http.Clients;
using Infrastructure.Http.Uri;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Retail.Api.Controllers;

namespace Retail.Api.Configuration;

public class DtoBasedRouteConvention : IControllerModelConvention
{
    private readonly RetailUrlResolver _retailUrlResolver = null!;

    public DtoBasedRouteConvention()
    {
        var options = new RetailClientSettings();
        var retailUrlResolver = new RetailUrlResolver(Options.Create(options));
        _retailUrlResolver = retailUrlResolver;
    }

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
        nameof(CashierController) => _retailUrlResolver.GetResourceName<CashierDto>(),
        nameof(ProductItemController) => _retailUrlResolver.GetResourceName<ProductItemDto>(),
        nameof(ReceiptController) => _retailUrlResolver.GetResourceName<ReceiptDto>(),
        nameof(StoreController) => _retailUrlResolver.GetResourceName<StoreDto>(),
        _ => throw new ArgumentException("Controller not found")
    };
}
