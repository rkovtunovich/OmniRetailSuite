using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/{resource}")]
public class ItemTypeController(IItemTypeService itemTypeService) : ControllerBase
{

    // GET api/v1/{resource}
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ProductTypeDto>>> CatalogTypesAsync()
    {
        var types = await itemTypeService.GetItemTypesAsync();

        return types;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProductTypeDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductTypeDto>> TypeByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        try
        {
            var type = await itemTypeService.GetItemTypeByIdAsync(id);
            return type;
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    //POST api/v1/{resource}
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateTypeAsync([FromBody] ProductTypeDto type)
    {
        try
        {
            var createdType = await itemTypeService.CreateItemTypeAsync(type);
            return Created();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    //PUT api/v1/{resource}
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateTypeAsync([FromBody] ProductTypeDto typeToUpdate)
    {
        try
        {
            var updatedType = await itemTypeService.UpdateItemTypeAsync(typeToUpdate);
            return Ok(updatedType);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    //DELETE api/v1/{resource}/id
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteTypeAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        try
        {
            await itemTypeService.DeleteItemTypeAsync(id, useSoftDeleting);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
