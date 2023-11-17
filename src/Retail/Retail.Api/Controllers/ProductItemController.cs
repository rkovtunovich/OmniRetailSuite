using Microsoft.AspNetCore.Mvc;
using Retail.Application.Services.Abstraction;
using Retail.Core.DTOs;

namespace Retail.Api.Controllers;

public class ProductItemController(IProductItemService productItemService, ILogger<ProductItemController> logger): ControllerBase
{
    private readonly IProductItemService _productItemService = productItemService;
    private readonly ILogger<ProductItemController> _logger = logger;

    [HttpGet]
    [Route("catalog-items")]
    [ProducesResponseType(typeof(List<CatalogItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CatalogItemDto>>> GetProductItemsAsync()
    {
        try
        {
            var productItems = await _productItemService.GetProductItemsAsync();

            return Ok(productItems);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting product items");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("catalog-items/{id:Guid}")]
    [ProducesResponseType(typeof(CatalogItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CatalogItemDto>> GetProductItemAsync(Guid id)
    {
        try
        {
            var productItem = await _productItemService.GetProductItemAsync(id);

            if (productItem is null)         
                return NotFound();
            
            return Ok(productItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting product item");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
