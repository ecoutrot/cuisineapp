/**
    public Guid Id { get; set; }
    public Guid? IngredientId { get; set; }
    public decimal Quantity { get; set; }
    public Guid? UnitId { get; set; }
    public bool? Optional { get; set; }
 */
export type RecipeIngredient = {
  id: string | null;
  ingredientId: string | null;
  quantity: number | null;
  unitId: string | null;
  optional: boolean | null;
};
