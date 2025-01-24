import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Recipe } from "../../types/Recipe";
import { Ingredient } from "../../types/Ingredient";
import { fetchIngredientsApi } from "../../services/Ingredient";
import { fetchUnitsApi } from "../../services/Unit";
import { Unit } from "../../types/Unit";
import { RecipeCategory } from "../../types/RecipeCategory";
import { fetchRecipeCategoriesApi } from "../../services/RecipeCategory";
import RecipeImproveGeneratorForm from "../../components/Recipe/RecipeImproveGeneratorForm";
import { generateRecipeImproveApi } from "../../services/RecipeGenerator";
import { RiRobot2Line } from "react-icons/ri";

function RecipeImproveGenerator() {
  const { id } = useParams();

  const [newEdit, setNewRecipe] = useState<Recipe | null>(null);
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
      generateRecipeImproveApi(id)
        .then(async (recipeAI) => {
          await fetchIngredientsApi()
            .then((ingredients) => {
              if (!ingredients) return
              setIngredients(ingredients);
              if (recipeAI)
                recipeAI.id = null;
              setNewRecipe(recipeAI);
            })
            .catch((error: unknown) => {
              console.error(error);
            });
        })
        .catch((error: unknown) => {
          console.error(error);
        });
    }
  }, [id]);

  if (!newEdit) {
    return (
      <>
        <div className="mt-6 flex flex-col items-center justify-center">
          <RiRobot2Line className="mt-10 animate-bounce text-8xl text-green-600" />
        </div>
      </>
    );
  }

  return (
    <>
      <RecipeImproveGeneratorForm recipe={newEdit} ingredients={ingredients} units={units} recipeCategories={recipeCategories} />
    </>
  );
}

export default RecipeImproveGenerator;
