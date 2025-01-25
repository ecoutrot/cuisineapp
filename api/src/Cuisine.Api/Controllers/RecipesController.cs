using System.Security.Claims;
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
public class RecipesController(IRecipeService recipeService) : ControllerBase
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
    public async Task<Results<Ok<RecipeDTO[]>,UnauthorizedHttpResult,BadRequest<string>>> GetAll(int? page = null, int? limit = null, string? search = null)
    {
        try
        {
            var userId = GetUserId(User);
            if (userId is null || (Guid)userId == Guid.Empty)
            {
                return TypedResults.Unauthorized();
            }
            var recipes = await recipeService.GetRecipesAsync((Guid)userId, page, limit, search);
            var recipeDTOs = recipes?.Select(recipe => recipe.ToRecipeDTO()).ToArray();
            return TypedResults.Ok(recipeDTOs ?? []);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<RecipeDTO>,NotFound,BadRequest<string>>> GetById([FromRoute] Guid id)
    {
        try
        {
            var recipe = await recipeService.GetRecipeByIdAsync(id);
            if (recipe is null)
            {
                return TypedResults.NotFound();
            }
            var recipeDTO = recipe.ToRecipeDTO();
            return TypedResults.Ok(recipeDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<Results<Created<RecipeDTO>,UnauthorizedHttpResult,BadRequest<string>>> Add([FromBody] NewRecipeDTO newRecipeDTO)
    {
        try
        {
            var recipe = newRecipeDTO.ToNewEntity();
            var userId = GetUserId(User);
            if (userId is null || (Guid)userId == Guid.Empty)
            {
                return TypedResults.Unauthorized();
            }
            recipe.UserId = userId;
            var addedRecipe = await recipeService.AddRecipeAsync(recipe);
            var addedRecipeDTO = addedRecipe?.ToRecipeDTO();
            return TypedResults.Created(nameof(GetById), addedRecipeDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<RecipeDTO>,NotFound,UnauthorizedHttpResult,BadRequest<string>>> Update([FromRoute] Guid id, [FromBody] RecipeDTO recipeDTO)
    {
        try
        {
            var recipe = recipeDTO.ToEntity();
            if (id != recipe.Id)
            {
                return TypedResults.BadRequest("Id in the route does not match the id in the body");
            }
            var existingRecipe = await recipeService.GetRecipeByIdAsync(id);
            var userId = GetUserId(User);
            if (existingRecipe?.UserId != userId)
            {
                return TypedResults.Unauthorized();
            }
            var updatedRecipe = await recipeService.UpdateRecipeAsync(id, recipe);
            if (updatedRecipe is null)
            {
                return TypedResults.NotFound();
            }
            var updatedRecipeDTO = updatedRecipe.ToRecipeDTO();
            return TypedResults.Ok(updatedRecipeDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<Results<NoContent,UnauthorizedHttpResult,BadRequest<string>>> Delete([FromRoute] Guid id)
    {
        try
        {
            var userId = GetUserId(User);
            var recipe = await recipeService.GetRecipeByIdAsync(id);
            if (userId is null || (Guid)userId == Guid.Empty || recipe?.UserId != userId)
            {
                return TypedResults.Unauthorized();
            }
            await recipeService.DeleteRecipeAsync((Guid)userId, id);
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
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
