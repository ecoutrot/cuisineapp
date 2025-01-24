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
public class IngredientsController(IIngredientService ingredientService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IngredientDTO[]>> GetAll(int? page = null, int? limit = null, string? search = null)
    {
        var ingredients = await ingredientService.GetIngredientsAsync(page, limit, search);
        var ingredientDTOs = ingredients?.Select(ingredient => ingredient.ToIngredientDTO()).ToArray();
        return Ok(ingredientDTOs ?? Array.Empty<IngredientDTO>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IngredientDTO>> GetById([FromRoute] Guid id)
    {
        var ingredient = await ingredientService.GetIngredientByIdAsync(id);
        if (ingredient is null)
        {
            return NotFound();
        }
        var ingredientDTO = ingredient.ToIngredientDTO();
        return Ok(ingredientDTO);
    }

    [HttpPost]
    public async Task<ActionResult<IngredientDTO>> Add([FromBody] IngredientDTO ingredientDTO)
    {
        try
        {
            var ingredient = ingredientDTO.ToEntity();
            var addedIngredient = await ingredientService.AddIngredientAsync(ingredient);
            var addedIngredientDTO = addedIngredient?.ToIngredientDTO();
            return CreatedAtAction(nameof(GetById), new { id = addedIngredientDTO?.Id }, addedIngredientDTO);
        }
        catch (AlreadyExistsException ex)
        {
            return StatusCode(StatusCodes.Status409Conflict, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<IngredientDTO>> Update([FromRoute] Guid id, [FromBody] IngredientDTO ingredientDTO)
    {
        var ingredient = ingredientDTO.ToEntity();
        var updatedIngredient = await ingredientService.UpdateIngredientAsync(id, ingredient);
        if (updatedIngredient is null)
        {
            return NotFound();
        }
        var updatedIngredientDTO = updatedIngredient.ToIngredientDTO();
        return Ok(updatedIngredientDTO);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await ingredientService.DeleteIngredientAsync(id);
        return NoContent();
    }
}
