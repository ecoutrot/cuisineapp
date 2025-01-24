using System.Text.RegularExpressions;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/ingredients/[controller]")]
public class RequeteController(IIngredientRepository ingredientRepository) : ControllerBase
{
    [HttpGet("{lettre}/{numero}")]
    public async Task<ActionResult<List<Ingredient>?>> Requete(string lettre, int numero)
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
                    return NotFound($"La page {pageUrl} n'existe pas.");
                }

                var listeIngredients = new List<Ingredient>();

                var html = await response.Content.ReadAsStringAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//a[@title]");
                if (nodes == null) return Ok(listeIngredients);

                var titles = nodes.Select(node => node.GetAttributeValue("title", "")).ToList();

                var listeIngredientsAll = await ingredientRepository.GetIngredientsAsync();

                foreach (var title in titles)
                {
                    if (listeIngredientsAll is not null && listeIngredientsAll.Any(ingredient => ingredient.Name != null && ingredient.Name == title))
                    {
                        continue;
                    }
                    var ingredient = new Ingredient { Name = title };
                    listeIngredients.Add(ingredient);
                }
                return Ok(listeIngredients);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors du traitement de l'URL {pageUrl}: {ex.Message}");
            }
        }
        return BadRequest("URL non spécifiée");
    }
}
