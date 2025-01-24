namespace Cuisine.Domain.Entities;

public class Unit
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = string.Empty;
    public List<RecipeIngredient>? RecipeIngredients { get; set; }
}