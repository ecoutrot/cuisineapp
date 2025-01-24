namespace Cuisine.Domain.Entities;

public class Ingredient
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<RecipeIngredient>? RecipeIngredients { get; set; }
    public Guid? IngredientCategoryId { get; set; }
    public IngredientCategory? IngredientCategory { get; set; }
}