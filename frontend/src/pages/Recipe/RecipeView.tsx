import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import RecipeCardItem from "../../components/Recipe/RecipeCardItem";
import { Recipe } from "../../types/Recipe";
import { fetchRecipeApi } from "../../services/Recipe";
import { fetchIngredientsApi } from "../../services/Ingredient";
import { Ingredient } from "../../types/Ingredient";
import { fetchUnitsApi } from "../../services/Unit";
import { Unit } from "../../types/Unit";
import Spinner from "../../components/Elements/Spinner";

function RecipeView() {
  const { id } = useParams();

  const [recipe, setRecipeToEdit] = useState<Recipe>();
  const [ingredients, setIngredients] = useState<Ingredient[]>([]);
  const [units, setUnits] = useState<Unit[]>([]);

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

    if (!id) {
      return;
    }
    fetchRecipeApi(id)
      .then((recipe) => {
        if (!recipe) return;
        setRecipeToEdit(recipe);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, [id]);

  if (!recipe) {
    return <Spinner />;
  }

  return (
    <>
      <RecipeCardItem recipe={recipe} ingredients={ingredients} units={units} />
    </>
  );
}

export default RecipeView;
