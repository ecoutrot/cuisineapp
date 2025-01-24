using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuisine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UnitsController(IUnitService unitService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UnitDTO[]>> GetAll(int? page = null, int? limit = null)
    {
        var units = await unitService.GetUnitsAsync(page, limit);
        var unitDTOs = units?.Select(unit => unit.ToUnitDTO()).ToArray();
        return Ok(unitDTOs ?? Array.Empty<UnitDTO>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UnitDTO>> GetById([FromRoute] Guid id)
    {
        var unit = await unitService.GetUnitByIdAsync(id);
        if (unit is null)
        {
            return NotFound();
        }
        var unitDTO = unit.ToUnitDTO();
        return Ok(unitDTO);
    }

    [HttpPost]
    public async Task<ActionResult<UnitDTO>> Add([FromBody] UnitDTO unitDTO)
    {
        try
        {
            var unit = unitDTO.ToEntity();
            var addedUnit = await unitService.AddUnitAsync(unit);
            var addedUnitDTO = addedUnit?.ToUnitDTO();
            return CreatedAtAction(nameof(GetById), new { id = addedUnitDTO?.Id }, addedUnitDTO);
        }
        catch (AlreadyExistsException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UnitDTO>> Update([FromRoute] Guid id, [FromBody] UnitDTO unitDTO)
    {
        var unit = unitDTO.ToEntity();
        var updatedUnit = await unitService.UpdateUnitAsync(id, unit);
        if (updatedUnit is null)
        {
            return NotFound();
        }
        var updatedUnitDTO = updatedUnit.ToUnitDTO();
        return Ok(updatedUnitDTO);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await unitService.DeleteUnitAsync(id);
        return NoContent();
    }
}
