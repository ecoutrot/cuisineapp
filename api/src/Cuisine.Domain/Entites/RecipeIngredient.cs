using System.ComponentModel.DataAnnotations.Schema;

namespace Cuisine.Domain.Entities;

public class RecipeIngredient
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    public Guid? RecipeId { get; set; }
    public Recipe? Recipe { get; set; }
    public Guid IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }
    public Guid? UnitId { get; set; }
    public Unit? Unit { get; set; }
    public bool? Optional { get; set; }
}