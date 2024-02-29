using Infrastructure.Http.Clients;
using Infrastructure.Http.Uri;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using ProductCatalog.Api.Controllers;

namespace ProductCatalog.Api.Configuration;

public class DtoBasedRouteConvention : IControllerModelConvention
{
    private readonly ProductCatalogUriResolver _productCatalogUriResolver = null!;

    public DtoBasedRouteConvention()
    {
        var options = new ProductCatalogClientSettings();
        var productCatalogUriResolver = new ProductCatalogUriResolver(Options.Create(options));
        _productCatalogUriResolver = productCatalogUriResolver;
    }

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
        nameof(BrandController) => _productCatalogUriResolver.GetResourceName<ProductBrandDto>(),
        nameof(ItemTypeController) => _productCatalogUriResolver.GetResourceName<ProductTypeDto>(),
        nameof(ItemController) => _productCatalogUriResolver.GetResourceName<ProductItemDto>(),
        nameof(ItemParentController) => _productCatalogUriResolver.GetResourceName<ProductParentDto>(),
        _ => throw new ArgumentException("Controller not found")
    };
}
