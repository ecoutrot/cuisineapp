namespace Cuisine.Domain.Entities;

public class RecipeCategory
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = string.Empty;
    public List<Recipe>? Recipes { get; set; }
}
