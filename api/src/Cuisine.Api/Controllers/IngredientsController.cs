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
    public async Task<Results<Ok<List<IngredientDTO>>,BadRequest<string>>> GetAll(int? page = null, int? limit = null, string? search = null)
    {
        try
        {
            var ingredientDTOs = await ingredientService.GetIngredientsAsync(page, limit, search);
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
            var ingredientDTO = await ingredientService.GetIngredientByIdAsync(id);
            if (ingredientDTO is null)
            {
                return TypedResults.NotFound();
            }
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
            var addedIngredientDTO = await ingredientService.AddIngredientAsync(newIngredientDTO);
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
            var updatedIngredientDTO = await ingredientService.UpdateIngredientAsync(id, ingredientDTO);
            if (updatedIngredientDTO is null)
            {
                return TypedResults.NotFound();
            }
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
