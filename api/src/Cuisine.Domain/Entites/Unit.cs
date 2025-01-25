namespace Cuisine.Domain.Entities;

public class Unit
{
    public required Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public List<RecipeIngredient>? RecipeIngredients { get; set; }
}