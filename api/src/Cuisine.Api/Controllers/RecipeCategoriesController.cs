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
public class RecipeCategoriesController(IRecipeCategoryService recipeCategoryService) : ControllerBase
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
    public async Task<Results<Ok<RecipeCategoryDTO[]>,BadRequest<string>>> GetAll(int? page = null, int? limit = null)
    {
        try
        {
            var recipeCategories = await recipeCategoryService.GetRecipeCategoriesAsync(page, limit);
            var recipeCategoryDTOs = recipeCategories?.Select(recipeCategory => recipeCategory.ToRecipeCategoryDTO()).ToArray();
            return TypedResults.Ok(recipeCategoryDTOs ?? []);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<RecipeCategoryDTO>,NotFound,BadRequest<string>>> GetById([FromRoute] Guid id)
    {
        try
        {
            var recipeCategory = await recipeCategoryService.GetRecipeCategoryByIdAsync(id);
            if (recipeCategory is null)
            {
                return TypedResults.NotFound();
            }
            var recipeCategoryDTO = recipeCategory.ToRecipeCategoryDTO();
            return TypedResults.Ok(recipeCategoryDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<Results<Created<RecipeCategoryDTO>,BadRequest<string>>> Add([FromBody] NewRecipeCategoryDTO newRecipeCategoryDTO)
    {
        try
        {
            var recipeCategory = newRecipeCategoryDTO.ToNewEntity();
            var addedRecipeCategory = await recipeCategoryService.AddRecipeCategoryAsync(recipeCategory);
            var addedRecipeCategoryDTO = addedRecipeCategory?.ToRecipeCategoryDTO();
            return TypedResults.Created(nameof(GetById), addedRecipeCategoryDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<RecipeCategoryDTO>,NotFound,BadRequest<string>>> Update([FromRoute] Guid id, [FromBody] RecipeCategoryDTO recipeCategoryDTO)
    {
        try
        {
            var recipeCategory = recipeCategoryDTO.ToEntity();
            var updatedRecipeCategory = await recipeCategoryService.UpdateRecipeCategoryAsync(id, recipeCategory);
            if (updatedRecipeCategory is null)
            {
                return TypedResults.NotFound();
            }
            var updatedRecipeCategoryDTO = updatedRecipeCategory.ToRecipeCategoryDTO();
            return TypedResults.Ok(updatedRecipeCategoryDTO);
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
            await recipeCategoryService.DeleteRecipeCategoryAsync(id);
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
