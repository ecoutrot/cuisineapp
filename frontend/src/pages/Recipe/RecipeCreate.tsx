import { useEffect, useState } from "react";
import { fetchIngredientsApi } from "../../services/Ingredient";
import { Ingredient } from "../../types/Ingredient";
import { fetchUnitsApi } from "../../services/Unit";
import { Recipe } from "../../types/Recipe";
import { Unit } from "../../types/Unit";
import RecipeForm from "../../components/Recipe/RecipeForm";
import { RecipeCategory } from "../../types/RecipeCategory";
import { fetchRecipeCategoriesApi } from "../../services/RecipeCategory";

function RecipeCreate() {
  const newRecipe: Recipe | null = null;
  const [ingredients, setIngredients] = useState<Ingredient[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);
  const [recipeCategories, setRecipeCategories] = useState<RecipeCategory[]>([]);

  useEffect(() => {
    fetchIngredientsApi()
      .then((ingredients) => {
        if (!ingredients) return;
        setIngredients(ingredients);
      })
      .catch((error: unknown) => {
        console.error(error);
      });

    fetchRecipeCategoriesApi()
      .then((recipeCategories) => {
        if (!recipeCategories) return;
        setRecipeCategories(recipeCategories);
      })
      .catch((error: unknown) => {
        console.error(error);
      });

    fetchUnitsApi()
      .then((units) => {
        if (!units) return;
        setUnits(units);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, []);

  return (
    <>
      <RecipeForm recipe={newRecipe} ingredients={ingredients} units={units} recipeCategories={recipeCategories} />
    </>
  );
}

export default RecipeCreate;
