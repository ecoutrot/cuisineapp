
import { Ingredient } from "../../types/Ingredient";
import IngredientForm from "../../components/Ingredient/IngredientForm";


function IngredientCreate() {

  const newIngredient: Ingredient | null = null;

  return (
    <>
      <IngredientForm ingredient={newIngredient} />
    </>
  );
}

export default IngredientCreate;
