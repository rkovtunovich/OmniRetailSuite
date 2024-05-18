using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/{resource}")]
public class BrandController(IBrandService brandService) : ControllerBase
{

    // GET api/v1/{resource}
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductBrandDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<ProductBrandDto>>> CatalogBrandsAsync()
    {
        var brands = await brandService.GetBrandsAsync();

        return brands;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProductBrandDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductBrandDto>> BrandByIdAsync([FromRoute]Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var brand = await brandService.GetBrandByIdAsync(id);

        if (brand is not null)
            return brand;

        return NotFound();
    }

    //POST api/v1/{resource}
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateBrandAsync([FromBody] ProductBrandDto brand)
    {
        var createdBrand = await brandService.CreateBrandAsync(brand);
        if (createdBrand.Id != Guid.Empty)
            return Created();

        return BadRequest();
    }

    //PUT api/v1/{resource}
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateBrandAsync([FromBody] ProductBrandDto brandToUpdate)
    {
        try
        {
            await brandService.UpdateBrandAsync(brandToUpdate);
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    //DELETE api/v1/{resource}/id
    [Route("{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteBrandAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        try
        {
            await brandService.DeleteBrandAsync(id, useSoftDeleting);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
