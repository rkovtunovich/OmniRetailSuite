using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class ItemParentController : ControllerBase
{
    private readonly IItemParentService _itemParentService;
    private readonly ILogger<ItemParentController> _logger;

    public ItemParentController(IItemParentService itemParentService, ILogger<ItemParentController> logger)
    {
        _itemParentService = itemParentService;
        _logger = logger;
    }

    // GET api/v1/[controller]/CatalogParents
    [HttpGet]
    [Route("parents")]
    [ProducesResponseType(typeof(List<ItemParentDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ItemParentDto>>> CatalogParentsAsync()
    {
        var parents = await _itemParentService.GetItemParentsAsync();

        return Ok(parents);
    }

    [HttpGet]
    [Route("parents/{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ItemParentDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ItemParentDto>> ParentByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var parent = await _itemParentService.GetItemParentByIdAsync(id);

        if (parent is null)
            return NotFound();

        return parent;
    }

    //POST api/v1/[controller]/parents
    [Route("parents")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateParentAsync([FromBody] ItemParentDto parent)
    {
        try
        {
            await _itemParentService.CreateItemParentAsync(parent);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating parent");
            return BadRequest();
        }
    }

    //PUT api/v1/[controller]/parents
    [Route("parents")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateParentAsync([FromBody] ItemParentDto parentToUpdate)
    {
        try
        {
            var parent = await _itemParentService.UpdateItemParentAsync(parentToUpdate);
            if (parent is null)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating parent");
            return BadRequest();
        }
    }

    //DELETE api/v1/[controller]/parents/id
    [Route("parents/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteParentAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        try
        {
            await _itemParentService.DeleteItemParentAsync(id, useSoftDeleting);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting parent");
            return NotFound();
        }
    }
}
