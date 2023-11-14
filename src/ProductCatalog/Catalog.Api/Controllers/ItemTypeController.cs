using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class ItemTypeController : ControllerBase
{
    private readonly IItemTypeService _itemTypeService;

    public ItemTypeController(IItemTypeService itemTypeService)
    {
        _itemTypeService = itemTypeService;
    }

    // GET api/v1/[controller]/CatalogTypes
    [HttpGet]
    [Route("types")]
    [ProducesResponseType(typeof(List<ItemTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ItemTypeDto>>> CatalogTypesAsync()
    {
        var types = await _itemTypeService.GetItemTypesAsync();

        return types;
    }

    [HttpGet]
    [Route("types/{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ItemTypeDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ItemTypeDto>> TypeByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        try
        {
            var type = await _itemTypeService.GetItemTypeByIdAsync(id);
            return type;
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    //POST api/v1/[controller]/types
    [Route("types")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateTypeAsync([FromBody] ItemTypeDto type)
    {
        try
        {
            var createdType = await _itemTypeService.CreateItemTypeAsync(type);
            return Created();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    //PUT api/v1/[controller]/types
    [Route("types")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateTypeAsync([FromBody] ItemTypeDto typeToUpdate)
    {
        try
        {
            var updatedType = await _itemTypeService.UpdateItemTypeAsync(typeToUpdate);
            return Ok(updatedType);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    //DELETE api/v1/[controller]/types/id
    [Route("types/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteTypeAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        try
        {
            await _itemTypeService.DeleteItemTypeAsync(id, useSoftDeleting);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
