
using Cuisine.Application.DTOs;
using Cuisine.Application.Interfaces;
using Cuisine.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


[ApiController]
[Authorize]
[ValidateInputs]
[Route("api/[controller]")]
public class RecipeGeneratorController(GeminiApiService geminiApiService, IConfiguration configuration, IPromptService promptService) : ControllerBase
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

    [HttpPost("idea")]
    public async Task<Results<Ok<NewRecipeDTO>,BadRequest<string>>> GenerateContentIdea([FromBody] string idea)
    {
        try
        {
            var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";
            var prompt = promptService.PromoptIdea(idea);
            var result = await geminiApiService.GenerateContentAsync(apiKey, prompt);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost("list")]
    public async Task<Results<Ok<NewRecipeDTO>,BadRequest<string>>> GenerateContentList([FromBody] string[] listIngredients)
    {
        try
        {
            var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";
            var prompt = promptService.PromptListIngredients(listIngredients);
            var result = await geminiApiService.GenerateContentAsync(apiKey, prompt);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }


    [HttpGet("improve/{id:guid}")]
    public async Task<Results<Ok<NewRecipeDTO>,BadRequest<string>>> GenerateContentImprove([FromRoute] Guid id)
    {
        try
        {
            var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";
            var prompt = await promptService.PromptImproveAsync(id);
            var result = await geminiApiService.GenerateContentAsync(apiKey, prompt);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }


    /* [HttpPost("image")]
    public async Task<ActionResult<NewRecipeDTO>> GenerateContentImage([FromForm] string title, [FromForm] IFormFile image)
    {
        if (image == null || string.IsNullOrEmpty(title))
        {
            return BadRequest("Both file and text are required.");
        }

        var fileExtension = Path.GetExtension(image.FileName);
        if (string.IsNullOrEmpty(fileExtension) || !new[] { ".jpg", ".png", ".jpeg" }.Contains(fileExtension.ToLower()))
        {
            return BadRequest("Unsupported file extension.");
        }

        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{fileExtension}");

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";

        var prompt = promptService.PromptImage(title);

        try
        {
            var result = await geminiApiService.GenerateContentAsync(apiKey, prompt, filePath);
            return Ok(result);
        }
        finally
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    } */


}
