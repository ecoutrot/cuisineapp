import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchRecipeApi } from "../../services/Recipe";
import { Recipe } from "../../types/Recipe";
import { Ingredient } from "../../types/Ingredient";
import { fetchIngredientsApi } from "../../services/Ingredient";
import { fetchUnitsApi } from "../../services/Unit";
import { Unit } from "../../types/Unit";
import RecipeForm from "../../components/Recipe/RecipeForm";
import { RecipeCategory } from "../../types/RecipeCategory";
import { fetchRecipeCategoriesApi } from "../../services/RecipeCategory";
import Spinner from "../../components/Elements/Spinner";

function RecipeEdit() {
  const { id } = useParams();

  const [recipeToEdit, setRecipeToEdit] = useState<Recipe>();
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

    fetchUnitsApi()
      .then((units) => {
        if (!units) return;
        setUnits(units);
      })
      .catch((error: unknown) => {
        console.error(error);
      });

    fetchRecipeCategoriesApi()
      .then((recipeCategories) => {
        if (!recipeCategories) return
        setRecipeCategories(recipeCategories);
      })
      .catch((error: unknown) => {
        console.error(error);
      });

    if (id) {
      fetchRecipeApi(id)
        .then((recipe) => {
          if (!recipe) return
          setRecipeToEdit(recipe);
        })
        .catch((error: unknown) => {
          console.error(error);
        });
    }
  }, [id]);

  if (!recipeToEdit) {
    return <Spinner />;
  }

  return (
    <>
      <RecipeForm recipe={recipeToEdit} ingredients={ingredients} units={units} recipeCategories={recipeCategories} />
    </>
  );
}

export default RecipeEdit;
