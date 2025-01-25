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
public class IngredientsController(IIngredientService ingredientService) : ControllerBase
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
    public async Task<Results<Ok<IngredientDTO[]>,BadRequest<string>>> GetAll(int? page = null, int? limit = null, string? search = null)
    {
        try
        {
            var ingredients = await ingredientService.GetIngredientsAsync(page, limit, search);
            var ingredientDTOs = ingredients?.Select(ingredient => ingredient.ToIngredientDTO()).ToArray();
            return TypedResults.Ok(ingredientDTOs ?? []);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<IngredientDTO>,NotFound,BadRequest<string>>> GetById([FromRoute] Guid id)
    {
        try
        {
            var ingredient = await ingredientService.GetIngredientByIdAsync(id);
            if (ingredient is null)
            {
                return TypedResults.NotFound();
            }
            var ingredientDTO = ingredient.ToIngredientDTO();
            return TypedResults.Ok(ingredientDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<Results<Created<IngredientDTO>,BadRequest<string>>> Add([FromBody] NewIngredientDTO newIngredientDTO)
    {
        try
        {
            var ingredient = newIngredientDTO.ToNewEntity();
            var addedIngredient = await ingredientService.AddIngredientAsync(ingredient);
            var addedIngredientDTO = addedIngredient?.ToIngredientDTO();
            return TypedResults.Created(nameof(GetById), addedIngredientDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<Results<Ok<IngredientDTO>,NotFound,BadRequest<string>>> Update([FromRoute] Guid id, [FromBody] IngredientDTO ingredientDTO)
    {
        try
        {
            var ingredient = ingredientDTO.ToEntity();
            var updatedIngredient = await ingredientService.UpdateIngredientAsync(id, ingredient);
            if (updatedIngredient is null)
            {
                return TypedResults.NotFound();
            }
            var updatedIngredientDTO = updatedIngredient.ToIngredientDTO();
            return TypedResults.Ok(updatedIngredientDTO);
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
            await ingredientService.DeleteIngredientAsync(id);
            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
