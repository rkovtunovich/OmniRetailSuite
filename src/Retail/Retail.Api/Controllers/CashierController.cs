using Contracts.Dtos.Retail;
using Microsoft.AspNetCore.Mvc;

namespace Retail.Api.Controllers;

[ApiController]
[Route("api/v1/retail")]
public class CashierController(ICashierService cashierService, ILogger<CashierController> logger) : ControllerBase
{
    private readonly ICashierService _cashierService = cashierService;
    private readonly ILogger<CashierController> _logger = logger;

    [HttpGet]
    [Route("cashiers")]
    [ProducesResponseType(typeof(List<CashierDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<CashierDto>>> GetCashiersAsync()
    {
        try
        {
            var cashiers = await _cashierService.GetCashiersAsync();

            return Ok(cashiers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting cashiers");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet]
    [Route("cashiers/{id:Guid}")]
    [ProducesResponseType(typeof(CashierDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CashierDto>> GetCashierAsync(Guid id)
    {
        try
        {
            var cashier = await _cashierService.GetCashierAsync(id);

            if (cashier is null)
                return NotFound();

            return Ok(cashier);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting cashier");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost]
    [Route("cashiers")]
    [ProducesResponseType(typeof(Cashier), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CashierDto>> CreateCashierAsync(CashierDto cashierDto)
    {
        try
        {
            var createdCashier = await _cashierService.CreateCashierAsync(cashierDto);

            return Created();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating cashier");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPut]
    [Route("cashiers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CashierDto>> UpdateCashierAsync(CashierDto cashierDto)
    {
        try
        {
            await _cashierService.UpdateCashierAsync(cashierDto);

            return Ok(cashierDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating cashier");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete]
    [Route("cashiers/{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CashierDto>> DeleteCashierAsync(Guid id, bool useSoftDeleting)
    {
        try
        {
            await _cashierService.DeleteCashierAsync(id, useSoftDeleting);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting cashier");

            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
