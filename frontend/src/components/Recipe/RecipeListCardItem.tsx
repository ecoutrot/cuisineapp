import { Recipe } from "../../types/Recipe";
import { Link } from "react-router-dom";
import clock from "../../assets/icons/clock.svg";
import fire from "../../assets/icons/fire.svg";
import chef from "../../assets/icons/chef.svg";
import euro from "../../assets/icons/euro.svg";
import { CookingType } from "../../types/enums/CookingType";
import { FaEye, FaPencilAlt } from "react-icons/fa";
import ListIcons from "../Elements/ListIcons";
import { RecipeCategory } from "../../types/RecipeCategory";
import { useState } from "react";
import { deleteRecipeApi } from "../../services/Recipe";
import { FaRegTrashCan } from "react-icons/fa6";

function RecipeListCardItem({ recipe, recipeCategory, onDelete }: { recipe: Recipe; recipeCategory: RecipeCategory | null; onDelete: (id: string) => void }) {
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    if (!window.confirm(`Êtes-vous sûr de vouloir supprimer "${recipe.name}" ?`)) {
      return;
    }

    setIsDeleting(true);
    if (!recipe.id) {
      setIsDeleting(false);
      return;
    }
    try {
      await deleteRecipeApi(recipe.id);
      onDelete(recipe.id);
    } catch (error) {
      console.error("Error deleting recipe:", error);
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <div id={recipe.id ?? ""} className="flex w-full items-center gap-4 rounded-md border border-gray-200 p-4 shadow-sm transition-all hover:bg-gray-50">
      <div className="grow">
        <div className="flex items-start justify-between gap-4">
          <div>
            {recipeCategory && <span className="inline-block rounded-md bg-indigo-50 px-2 py-1 text-xs font-medium text-indigo-700 ring-1 ring-indigo-500/10">{recipeCategory.name}</span>}
            <h3 className="mt-2 text-lg font-bold text-gray-800">{recipe.name}</h3>
          </div>
          <div className="m-2 items-center gap-2 text-sm text-gray-500">
            {!!recipe.preparationTime && (
              <div className="flex items-center gap-1">
                <img src={clock} alt="Préparation" className="size-5" />
                <span>{recipe.preparationTime} min</span>
              </div>
            )}
            {!!recipe.cookingTime && (
              <div className="flex items-center gap-1">
                <img src={fire} alt="Cuisson" className="size-5" />
                <span>{recipe.cookingTime} min</span>
              </div>
            )}
          </div>
        </div>
        <div className="mt-4 flex items-center gap-4 text-gray-600">
          {!!recipe.cookingType && <img src={CookingType[recipe.cookingType]} alt="Type de cuisson" className="size-6" />}
          <span>
            <ListIcons index={recipe.difficulty ?? 0} icon={chef} />
          </span>
          <span>
            <ListIcons index={recipe.price ?? 0} icon={euro} />
          </span>
        </div>
        {recipe.description && <p className="mt-4 text-sm text-gray-600">{recipe.description}</p>}
      </div>
      <div className="flex flex-col items-center gap-2">
        <Link
          to={`/recipes/${recipe.id}`}
          className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-indigo-600 hover:bg-indigo-100 focus:outline-none focus:ring focus:ring-indigo-300"
          title="Voir la recette"
        >
          <FaEye />
        </Link>
        <Link
          to={`/recipes/edit/${recipe.id}`}
          className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-indigo-600 hover:bg-indigo-100 focus:outline-none focus:ring focus:ring-indigo-300"
          title="Éditer la recette"
        >
          <FaPencilAlt />
        </Link>
        <button
          className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-red-600 hover:bg-red-600 hover:text-white focus:outline-none focus:ring"
          type="button"
          onClick={() => void handleDelete()}
          disabled={isDeleting}
        >
          <FaRegTrashCan />
        </button>
      </div>
    </div>
  );
}

export default RecipeListCardItem;
