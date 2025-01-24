import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { fetchRecipeCategoryApi } from "../../services/RecipeCategory";
import { RecipeCategory } from "../../types/RecipeCategory";
import RecipeCategoryForm from "../../components/RecipeCategory/RecipeCategoryForm";
import Spinner from "../../components/Elements/Spinner";

function RecipeCategoryEdit() {
  const { id } = useParams();

  const [recipeCategoryToEdit, setRecipeCategoryToEdit] = useState<RecipeCategory>();

  useEffect(() => {
    if (id) {
      fetchRecipeCategoryApi(id)
        .then((recipe) => {
          if (!recipe) return;
          setRecipeCategoryToEdit(recipe);
        })
        .catch((error: unknown) => {
          console.error(error);
        });
    }
  }, [id]);

  if (!recipeCategoryToEdit) {
    return <Spinner />;
  }

  return (
    <>
      <RecipeCategoryForm recipeCategory={recipeCategoryToEdit} />
    </>
  );
}

export default RecipeCategoryEdit;
