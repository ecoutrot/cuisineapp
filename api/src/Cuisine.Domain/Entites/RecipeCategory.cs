namespace Cuisine.Domain.Entities;

public class RecipeCategory
{
    public required Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public List<Recipe>? Recipes { get; set; }
}
