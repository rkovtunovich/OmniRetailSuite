using Contracts.Dtos.Retail;
using Microsoft.AspNetCore.Mvc;

namespace Retail.Api.Controllers;

[Route("api/v1/{resource}")]
public class ProductItemController(IProductItemService productItemService, ILogger<ProductItemController> logger): ControllerBase
{
    private readonly IProductItemService _productItemService = productItemService;
    private readonly ILogger<ProductItemController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<ProductItemDto>>> GetProductItemsAsync()
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
    [Route("{id:Guid}")]
    [ProducesResponseType(typeof(ProductItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductItemDto>> GetProductItemAsync(Guid id)
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
