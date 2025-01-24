namespace Cuisine.Domain.Entities;

public class IngredientCategory
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = string.Empty;
    public List<Ingredient>? Ingredients { get; set; }
}