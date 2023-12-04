using Microsoft.AspNetCore.Mvc;
using Retail.Application.Services.Abstraction;
using Retail.Core.DTOs;
using Retail.Core.Entities;

namespace Retail.Api.Controllers;

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

            return CreatedAtAction(nameof(GetCashierAsync), new { id = createdCashier.Id }, createdCashier);
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
}
