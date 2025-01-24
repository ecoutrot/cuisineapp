using System.Security.Claims;
using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Cuisine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RecipesController(IRecipeService recipeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RecipeDTO[]>> GetAll(int? page = null, int? limit = null, string? search = null)
    {
        var userId = GetUserId(User);
        if (userId is null || (Guid)userId == Guid.Empty)
        {
            return Unauthorized();
        }
        var recipes = await recipeService.GetRecipesAsync((Guid)userId, page, limit, search);
        var recipeDTOs = recipes?.Select(recipe => recipe.ToRecipeDTO()).ToArray();
        return Ok(recipeDTOs ?? Array.Empty<RecipeDTO>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RecipeDTO>> GetById([FromRoute] Guid id)
    {
        var recipe = await recipeService.GetRecipeByIdAsync(id);
        if (recipe is null)
        {
            return NotFound();
        }
        var recipeDTO = recipe.ToRecipeDTO();
        return Ok(recipeDTO);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDTO>> Add([FromBody] RecipeDTO recipeDTO)
    {
        try
        {
            var recipe = recipeDTO.ToEntity();
            var userId = GetUserId(User);
            if (userId is null)
            {
                return Unauthorized();
            }
            recipe.UserId = userId;
            var addedRecipe = await recipeService.AddRecipeAsync(recipe);
            var addedRecipeDTO = addedRecipe?.ToRecipeDTO();
            return CreatedAtAction(nameof(GetById), new { id = addedRecipeDTO?.Id }, addedRecipeDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<RecipeDTO>> Update([FromRoute] Guid id, [FromBody] RecipeDTO recipeDTO)
    {
        var recipe = recipeDTO.ToEntity();
        if (id != recipe.Id)
        {
            return BadRequest();
        }
        var existingRecipe = await recipeService.GetRecipeByIdAsync(id);
        var userId = GetUserId(User);
        if (existingRecipe?.UserId != userId)
        {
            return Unauthorized();
        }
        var updatedRecipe = await recipeService.UpdateRecipeAsync(id, recipe);
        if (updatedRecipe is null)
        {
            return NotFound();
        }
        var updatedRecipeDTO = updatedRecipe.ToRecipeDTO();
        return Ok(updatedRecipeDTO);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await recipeService.DeleteRecipeAsync(id);
        return NoContent();
    }

    private static Guid? GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return null;
        }
        return new Guid(userId);
    }
    
}
