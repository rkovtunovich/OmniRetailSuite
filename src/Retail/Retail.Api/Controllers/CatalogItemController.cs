using Microsoft.AspNetCore.Mvc;
using Retail.Application.Services.Abstraction;
using Retail.Core.DTOs;

namespace Retail.Api.Controllers;

public class CatalogItemController(ICatalogItemService catalogItemService, ILogger<CatalogItemController> logger): ControllerBase
{
    private readonly ICatalogItemService _catalogItemService = catalogItemService;
    private readonly ILogger<CatalogItemController> _logger = logger;

    [HttpGet]
    [Route("catalog-items")]
    [ProducesResponseType(typeof(List<CatalogItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CatalogItemDto>>> GetCatalogItemsAsync()
    {
        try
        {
            var catalogItems = await _catalogItemService.GetCatalogItemsAsync();

            return Ok(catalogItems);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting catalog items");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("catalog-items/{id:Guid}")]
    [ProducesResponseType(typeof(CatalogItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CatalogItemDto>> GetCatalogItemAsync(Guid id)
    {
        try
        {
            var catalogItem = await _catalogItemService.GetCatalogItemAsync(id);

            if (catalogItem is null)         
                return NotFound();
            
            return Ok(catalogItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting catalog item");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
