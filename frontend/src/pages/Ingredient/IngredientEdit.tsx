import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchIngredientApi } from "../../services/Ingredient";
import { Ingredient } from "../../types/Ingredient";
import IngredientForm from "../../components/Ingredient/IngredientForm";
import Spinner from "../../components/Elements/Spinner";

function IngredientEdit() {
  const { id } = useParams();

  const [ingredientToEdit, setIngredientToEdit] = useState<Ingredient>();

  useEffect(() => {
    if (id) {
      fetchIngredientApi(id)
        .then((recipe) => {
          if (!recipe) {
            return;
          }
          setIngredientToEdit(recipe);
        })
        .catch((error: unknown) => {
          console.error(error);
        });
    }
  }, [id]);

  if (!ingredientToEdit) {
    return <Spinner />;
  }

  return (
    <>
      <IngredientForm ingredient={ingredientToEdit} />
    </>
  );
}

export default IngredientEdit;
