using Cuisine.Application.DTOs;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


[ApiController]
[Authorize(Roles = "Admin")]
[ValidateInputs]
[Route("api/ingredients/[controller]")]
public class RequeteController(IIngredientRepository ingredientRepository) : ControllerBase
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
    
    [HttpGet("{lettre}/{numero}")]
    public async Task<Results<Ok<List<NewIngredientDTO>>,NotFound<string>,BadRequest<string>>> Requete(string lettre, int numero)
    {
        var baseUrl = "https://www.marmiton.org/recettes/index/ingredient";
        var pageUrl = $"{baseUrl}/{lettre}/{numero}";
        var httpClient = new HttpClient();

        if (!string.IsNullOrWhiteSpace(pageUrl))
        {
            try
            {
                var response = await httpClient.GetAsync(pageUrl);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return TypedResults.NotFound($"La page {pageUrl} n'existe pas.");
                }

                var listeIngredients = new List<NewIngredientDTO>();

                var html = await response.Content.ReadAsStringAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//a[@title]");
                if (nodes == null) return TypedResults.Ok(listeIngredients);

                var titles = nodes.Select(node => node.GetAttributeValue("title", "")).ToList();

                var listeIngredientsAll = await ingredientRepository.GetIngredientsAsync();

                foreach (var title in titles)
                {
                    if (string.IsNullOrEmpty(title) || (listeIngredientsAll is not null && listeIngredientsAll.Any(ingredient => ingredient.Name != null && ingredient.Name == title)))
                    {
                        continue;
                    }
                    var ingredient = new NewIngredientDTO { Name = title };
                    listeIngredients.Add(ingredient);
                }
                return TypedResults.Ok(listeIngredients);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest($"Erreur lors du traitement de l'URL {pageUrl}: {ex.Message}");
            }
        }
        return TypedResults.BadRequest("URL non spécifiée");
    }
}
