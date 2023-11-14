using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    // GET api/v1/[controller]/CatalogBrands
    [HttpGet]
    [Route("brands")]
    [ProducesResponseType(typeof(List<BrandDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<BrandDto>>> CatalogBrandsAsync()
    {
        var brands = await _brandService.GetBrandsAsync();

        return brands;
    }

    [HttpGet]
    [Route("brands/{id:Guid}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BrandDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<BrandDto>> BrandByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return BadRequest();

        var brand = await _brandService.GetBrandByIdAsync(id);

        if (brand is not null)
            return brand;

        return NotFound();
    }

    //POST api/v1/[controller]/brands
    [Route("brands")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateBrandAsync([FromBody] BrandDto brand)
    {
        var createdBrand = await _brandService.CreateBrandAsync(brand);
        if (createdBrand.Id != Guid.Empty)
            return Created();

        return BadRequest();
    }

    //PUT api/v1/[controller]/types
    [Route("brands")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateBrandAsync([FromBody] BrandDto brandToUpdate)
    {
        try
        {
            await _brandService.UpdateBrandAsync(brandToUpdate);
            return Ok();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    //DELETE api/v1/[controller]/brands/id
    [Route("brands/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteBrandAsync(Guid id, [FromQuery] bool useSoftDeleting = false)
    {
        try
        {
            await _brandService.DeleteBrandAsync(id, useSoftDeleting);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}
