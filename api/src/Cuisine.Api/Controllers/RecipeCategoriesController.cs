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
public class RecipeCategoriesController(IRecipeCategoryService recipeCategoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RecipeCategoryDTO[]>> GetAll(int? page = null, int? limit = null)
    {
        var recipeCategories = await recipeCategoryService.GetRecipeCategoriesAsync(page, limit);
        var recipeCategoryDTOs = recipeCategories?.Select(recipeCategory => recipeCategory.ToRecipeCategoryDTO()).ToArray();
        return Ok(recipeCategoryDTOs ?? Array.Empty<RecipeCategoryDTO>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeCategoryDTO>> GetById([FromRoute] Guid id)
    {
        var recipeCategory = await recipeCategoryService.GetRecipeCategoryByIdAsync(id);
        if (recipeCategory is null)
        {
            return NotFound();
        }
        var recipeCategoryDTO = recipeCategory.ToRecipeCategoryDTO();
        return Ok(recipeCategoryDTO);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeCategoryDTO>> Add([FromBody] RecipeCategoryDTO recipeCategoryDTO)
    {
        try
        {
            var recipeCategory = recipeCategoryDTO.ToEntity();
            var addedRecipeCategory = await recipeCategoryService.AddRecipeCategoryAsync(recipeCategory);
            var addedRecipeCategoryDTO = addedRecipeCategory?.ToRecipeCategoryDTO();
            return CreatedAtAction(nameof(GetById), new { id = addedRecipeCategoryDTO?.Id }, addedRecipeCategoryDTO);
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
    public async Task<ActionResult<RecipeCategoryDTO>> Update([FromRoute] Guid id, [FromBody] RecipeCategoryDTO recipeCategoryDTO)
    {
        var recipeCategory = recipeCategoryDTO.ToEntity();
        var updatedRecipeCategory = await recipeCategoryService.UpdateRecipeCategoryAsync(id, recipeCategory);
        if (updatedRecipeCategory is null)
        {
            return NotFound();
        }
        var updatedRecipeCategoryDTO = updatedRecipeCategory.ToRecipeCategoryDTO();
        return Ok(updatedRecipeCategoryDTO);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await recipeCategoryService.DeleteRecipeCategoryAsync(id);
        return NoContent();
    }
}
