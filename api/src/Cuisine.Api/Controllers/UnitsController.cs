using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuisine.Api.Controllers;

[ApiController]
[Authorize]
[ValidateInputs]
[Route("api/[controller]")]
public class UnitsController(IUnitService unitService) : ControllerBase
{
    /// <summary>
    /// Action filter that could be added either on method or controller to ensure that Model state validation method is called before executing
    /// </summary>
    public class ValidateInputsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }

    [HttpGet]
    public async Task<Results<Ok<UnitDTO[]>,BadRequest<string>>> GetAll(int? page = null, int? limit = null)
    {
        try
        {
            var units = await unitService.GetUnitsAsync(page, limit);
            var unitDTOs = units?.Select(unit => unit.ToUnitDTO()).ToArray();
            return TypedResults.Ok(unitDTOs ?? []);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<UnitDTO>,NotFound,BadRequest<string>>> GetById([FromRoute] Guid id)
    {
        try
        {
            var unit = await unitService.GetUnitByIdAsync(id);
            if (unit is null)
            {
                return TypedResults.NotFound();
            }
            var unitDTO = unit.ToUnitDTO();
            return TypedResults.Ok(unitDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<Results<Created<UnitDTO>,BadRequest<string>>> Add([FromBody] NewUnitDTO newUnitDTO)
    {
        try
        {
            var unit = newUnitDTO.ToNewEntity();
            var addedUnit = await unitService.AddUnitAsync(unit);
            var addedUnitDTO = addedUnit?.ToUnitDTO();
            return TypedResults.Created(nameof(GetById), addedUnitDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<UnitDTO>,NotFound,BadRequest<string>>> Update([FromRoute] Guid id, [FromBody] UnitDTO unitDTO)
    {
        try
        {
            var unit = unitDTO.ToEntity();
            var updatedUnit = await unitService.UpdateUnitAsync(id, unit);
            if (updatedUnit is null)
            {
                return TypedResults.NotFound();
            }
            var updatedUnitDTO = updatedUnit.ToUnitDTO();
            return TypedResults.Ok(updatedUnitDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<Results<NoContent,BadRequest<string>>> Delete([FromRoute] Guid id)
    {
        try
        {
            await unitService.DeleteUnitAsync(id);
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
