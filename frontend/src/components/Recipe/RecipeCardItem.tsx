import { Recipe } from "../../types/Recipe";
import { Ingredient } from "../../types/Ingredient";
import { Unit } from "../../types/Unit";
import { User } from "../../types/User";
import clock from "../../assets/icons/clock.svg";
import fire from "../../assets/icons/fire.svg";
import chef from "../../assets/icons/chef.svg";
import euro from "../../assets/icons/euro.svg";
import { CookingType, CookingTypeLabelArray } from "../../types/enums/CookingType";
import ListIcons from "../Elements/ListIcons";
import { fetchUserApi } from "../../services/User";
import { useEffect, useState } from "react";
import { BsStars } from "react-icons/bs";
import { Link } from "react-router-dom";

function RecipeCardItem({ recipe, ingredients, units }: { recipe: Recipe; ingredients: Ingredient[]; units: Unit[] }) {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    if (recipe.userId) {
      fetchUserApi(recipe.userId)
        .then((user) => {
          setUser(user);
        })
        .catch((error: unknown) => {
          console.error(error);
        });
    }
  }, [recipe.userId]);

  return (
    <div className="mx-auto mb-6 max-w-3xl rounded-lg border-t bg-white p-6 shadow-md">
      <section className="mb-6 border-b pb-6">
        <div className="flex items-center justify-between">
          <h1 className="text-2xl font-bold text-gray-800">{recipe.name}</h1>
          {recipe?.id && (
            <Link
              to={`/recipes/generator/${recipe.id}`}
              className="group ml-4 rounded-lg border border-green-600 bg-green-600 p-2.5 px-5 text-sm font-medium text-white shadow hover:bg-transparent hover:text-green-600 focus:outline-none focus:ring-2 focus:ring-green-600 active:text-green-500"
            >
              <div className="relative flex flex-col items-center">
                <BsStars className="text-xl" />
                <span className="absolute top-7 z-10 w-32 scale-0 text-center text-base text-green-600 group-hover:scale-100">Améliorer la recette</span>
              </div>
            </Link>
          )}
        </div>
        {user?.username && (
          <small>
            <em>{user.username}</em>
          </small>
        )}
        <p className="mt-3 text-gray-600">{recipe.description}</p>
      </section>
      <section className="mb-8">
        <h2 className="mb-4 text-lg font-semibold text-gray-800">Informations</h2>
        <div className="grid grid-cols-1 gap-x-6 gap-y-4 sm:grid-cols-2">
          {!!recipe.preparationTime && (
            <div className="flex items-center gap-x-3">
              <img src={clock} alt="Temps de préparation" className="size-8" />
              <p className="text-gray-900">
                <span className="mb-1">Préparation : {recipe.preparationTime} minutes</span>
              </p>
            </div>
          )}
          {!!recipe.cookingTime && (
            <div className="flex items-center gap-x-3">
              <img src={fire} alt="Temps de cuisson" className="size-8" />
              <p className="text-gray-900">
                <span className="mb-1">Cuisson : {recipe.cookingTime} minutes</span>
              </p>
            </div>
          )}
          {!!recipe.restTime && (
            <div className="flex items-center gap-x-3">
              <img src={clock} alt="Temps de repos" className="size-8" />
              <p className="text-gray-900">
                <span className="mb-1">Repos : {recipe.restTime} minutes</span>
              </p>
            </div>
          )}
          {!!recipe.cookingType && (
            <div className="flex items-center gap-x-3">
              <img src={CookingType[recipe.cookingType ?? 0]} alt="Type de cuisson" className="size-8" />
              <p className="text-gray-900">
                <span className="mb-1">Type de cuisson : {CookingTypeLabelArray[recipe.cookingType]}</span>
              </p>
            </div>
          )}
          <div className="flex items-center gap-x-3">
            <p className="flex items-center gap-2 text-gray-900">
              <span className="mb-1">Difficulté :</span>
              <ListIcons index={recipe.difficulty ?? 0} icon={chef} />
            </p>
          </div>
          <div className="flex items-center gap-x-3">
            <p className="flex items-center gap-2 text-gray-900">
              <span className="mb-1">Prix :</span>
              <ListIcons index={recipe.price ?? 0} icon={euro} />
            </p>
          </div>
        </div>
      </section>
      <section className="mb-8">
        <h2 className="text-lg font-semibold text-gray-800">Ingrédients</h2>
        {!!recipe.portions && recipe.portions > 0 && (
          <small className="text-gray-600">
            <em>{recipe.portions} personnes</em>
          </small>
        )}
        <ul className="mt-4 space-y-2">
          {recipe.recipeIngredients?.map((recipeIngredient, index) => {
            const ingredient = ingredients.find((ing) => ing.id === recipeIngredient.ingredientId);
            const unit = units.find((u) => u.id === recipeIngredient.unitId);

            return (
              <li key={`${recipeIngredient.ingredientId}-${index}`} className="flex items-center gap-x-2.5">
                <p className="text-gray-900">
                  {!!recipeIngredient.quantity && recipeIngredient.quantity} {unit?.name} {ingredient?.name} {recipeIngredient.optional && <em>(facultatif)</em>}
                </p>
              </li>
            );
          })}
        </ul>
      </section>
      <section className="mb-8">
        <h2 className="mb-4 text-lg font-semibold text-gray-800">Étapes</h2>
        <ol className="list-decimal space-y-4 pl-6 text-gray-900">
          {recipe.steps?.map((step, index) => (
            <li key={index}>{step}</li>
          ))}
        </ol>
      </section>
      {recipe.advice && (
        <footer className="mt-6 border-t pt-6">
          <h2 className="mb-2 text-lg font-semibold text-gray-800">Conseils</h2>
          <p className="text-sm text-gray-600">{recipe.advice}</p>
        </footer>
      )}
    </div>
  );
}

export default RecipeCardItem;
