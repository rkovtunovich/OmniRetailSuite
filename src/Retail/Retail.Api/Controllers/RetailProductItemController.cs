using Contracts.Dtos.Retail;
using Microsoft.AspNetCore.Mvc;

namespace Retail.Api.Controllers;

[Route("api/v1/{resource}")]
public class RetailProductItemController(IProductItemService productItemService, ILogger<RetailProductItemController> logger): ControllerBase
{
    private readonly IProductItemService _productItemService = productItemService;
    private readonly ILogger<RetailProductItemController> _logger = logger;

    [HttpGet]
    [ProducesResponseType(typeof(List<RetailProductItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<RetailProductItemDto>>> GetProductItemsAsync()
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
    [ProducesResponseType(typeof(RetailProductItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RetailProductItemDto>> GetProductItemAsync(Guid id)
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
