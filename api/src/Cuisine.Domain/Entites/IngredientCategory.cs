namespace Cuisine.Domain.Entities;

public class IngredientCategory
{
    public required Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public List<Ingredient>? Ingredients { get; set; }
}