import { useEffect, useState } from "react";
import { fetchIngredientsApi } from "../../services/Ingredient";
import { Ingredient } from "../../types/Ingredient";
import RecipeGeneratorForm from "../../components/Recipe/RecipeGeneratorForm";

function RecipeGenerator() {
  const [ingredients, setIngredients] = useState<Ingredient[]>([]);

  useEffect(() => {
    fetchIngredientsApi()
      .then((ingredients) => {
        if (!ingredients) return;
        setIngredients(ingredients);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, []);

  return (
    <>
      <RecipeGeneratorForm listeIngredients={ingredients} />
    </>
  );
}

export default RecipeGenerator;
