using Catalog.Application.DTOs.CatalogTDOs;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/catalog")]
public class BrandController: ControllerBase
{
    private readonly IBrandRepository _brandRepository;

    public BrandController(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    // GET api/v1/[controller]/CatalogBrands
    [HttpGet]
    [Route("brands")]
    [ProducesResponseType(typeof(List<BrandDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<List<BrandDto>>> CatalogBrandsAsync()
    {
        var brands = await _brandRepository.GetBrandsAsync();

        return brands.AsQueryable().Select(BrandDto.Projection).ToList();
    }

    [HttpGet]
    [Route("brands/{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BrandDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<BrandDto>> BrandByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest();

        var brand = await _brandRepository.GetBrandByIdAsync(id);

        if (brand is not null)
            return BrandDto.FromEntity(brand);

        return NotFound();
    }

    //POST api/v1/[controller]/brands
    [Route("brands")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> CreateBrandAsync([FromBody] BrandDto brand)
    {
        var brandEntity = brand.ToEntity();

        var isCreated = await _brandRepository.CreateBrandAsync(brandEntity);
        if (isCreated)
            return CreatedAtAction(nameof(BrandByIdAsync), new { id = brandEntity.Id }, null);

        return BadRequest();
    }

    //PUT api/v1/[controller]/types
    [Route("brands")]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateBrandAsync([FromBody] BrandDto brandToUpdate)
    {
        var brand = brandToUpdate.ToEntity();

        var isUpdated = await _brandRepository.UpdateBrandAsync(brand);
        if (isUpdated)
            return Ok();

        return BadRequest();
    }

    //DELETE api/v1/[controller]/brands/id
    [Route("brands/{id}")]
    [HttpDelete]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteBrandAsync(int id, [FromQuery] bool useSoftDeleting = false)
    {
        var isDeleted = await _brandRepository.DeleteBrandAsync(id, useSoftDeleting);
        if (isDeleted)
            return NoContent();

        return NotFound();
    }
}
