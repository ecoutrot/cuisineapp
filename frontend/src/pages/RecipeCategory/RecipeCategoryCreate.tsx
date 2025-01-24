
import { RecipeCategory } from "../../types/RecipeCategory";
import RecipeCategoryForm from "../../components/RecipeCategory/RecipeCategoryForm";


function RecipeCategoryCreate() {

  const newRecipeCategory: RecipeCategory | null = null;

  return (
    <>
      <RecipeCategoryForm recipeCategory={newRecipeCategory} />
    </>
  );
}

export default RecipeCategoryCreate;
