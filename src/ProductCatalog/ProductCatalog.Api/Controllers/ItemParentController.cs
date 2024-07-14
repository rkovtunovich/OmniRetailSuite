using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/{resource}")]
public class ItemParentController(IItemParentService itemParentService, ILogger<ItemParentController> logger) : ControllerBase
{
    // GET api/v1/{resource}
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductParentDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ProductParentDto>>> CatalogParentsAsync()
    {
        var parents = await itemParentService.GetItemParentsAsync();

        return Ok(parents);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProductParentDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductParentDto>> ParentByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var parent = await itemParentService.GetItemParentByIdAsync(id);

        if (parent is null)
            return NotFound();

        return parent;
    }

    //POST api/v1/{resource}
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateParentAsync([FromBody] ProductParentDto parent)
    {
        try
        {
            await itemParentService.CreateItemParentAsync(parent);
            return Created();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating parent");
            return BadRequest();
        }
    }

    //PUT api/v1/{resource}
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> UpdateParentAsync([FromBody] ProductParentDto parentToUpdate)
    {
        try
        {
            var parent = await itemParentService.UpdateItemParentAsync(parentToUpdate);
            if (parent is null)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while updating parent");
            return BadRequest();
        }
    }

    //DELETE api/v1/{resource}/id
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteParentAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        try
        {
            await itemParentService.DeleteItemParentAsync(id, useSoftDeleting);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while deleting parent");
            return NotFound();
        }
    }
}
