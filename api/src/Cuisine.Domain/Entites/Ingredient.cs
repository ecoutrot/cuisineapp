namespace Cuisine.Domain.Entities;

public class Ingredient
{
    public required Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<RecipeIngredient>? RecipeIngredients { get; set; }
    public Guid? IngredientCategoryId { get; set; }
    public IngredientCategory? IngredientCategory { get; set; }
}