
using Cuisine.Application.DTOs;
using Cuisine.Application.Interfaces;
using Cuisine.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RecipeGeneratorController(GeminiApiService geminiApiService, IConfiguration configuration, IPromptService promptService) : ControllerBase
{

    [HttpPost("idea")]
    public async Task<ActionResult<RecipeDTO>> GenerateContentIdea([FromBody] string idea)
    {
        var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";

        var prompt = promptService.PromoptIdea(idea);
        
        var result = await geminiApiService.GenerateContentAsync(apiKey, prompt);

        return Ok(result);
    }

    [HttpPost("list")]
    public async Task<ActionResult<RecipeDTO>> GenerateContentList([FromBody] string[] listIngredients)
    {
        var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";

        var prompt = promptService.PromptListIngredients(listIngredients);

        var result = await geminiApiService.GenerateContentAsync(apiKey, prompt);

        return Ok(result);
    }


    [HttpGet("improve/{id:guid}")]
    public async Task<ActionResult<RecipeDTO>> GenerateContentImprove([FromRoute] Guid id)
    {
        var apiKey = configuration["ApiKeys:GeminiApiKey"] ?? "";

        var prompt = await promptService.PromptImproveAsync(id);
        
        var result = await geminiApiService.GenerateContentAsync(apiKey, prompt);

        return Ok(result);
    }


    /* [HttpPost("image")]
    public async Task<ActionResult<RecipeDTO>> GenerateContentImage([FromForm] string title, [FromForm] IFormFile image)
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
