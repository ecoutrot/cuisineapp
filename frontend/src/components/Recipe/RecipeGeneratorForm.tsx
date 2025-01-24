import { useEffect, useState } from "react";
import { FaPlus } from "react-icons/fa";
import CuisineDataList from "../Forms/CuisineDataList";
import { Ingredient } from "../../types/Ingredient";
import RecipeForm from "./RecipeForm";
import { fetchRecipeCategoriesApi } from "../../services/RecipeCategory";
import { fetchUnitsApi } from "../../services/Unit";
import { Recipe } from "../../types/Recipe";
import { RecipeCategory } from "../../types/RecipeCategory";
import { Unit } from "../../types/Unit";
import { generateRecipeIdeaApi, generateRecipeImageApi, generateRecipeIngredientsApi } from "../../services/RecipeGenerator";
import { fetchIngredientsApi } from "../../services/Ingredient";
import CuisineInputText from "../Forms/CuisineInputText";
import { FaRegTrashCan } from "react-icons/fa6";
import CuisineInputFile from "../Forms/CuisineInputFile";
import { RiRobot2Line } from "react-icons/ri";

function RecipeGeneratorForm({ listeIngredients }: { listeIngredients: Ingredient[] }) {
  const [loading, setLoading] = useState<boolean>(false);
  const [listIngredients, setListIngredients] = useState<string[]>([]);
  const [ingredient, setIngredient] = useState<string | null>(null);
  const [idea, setIdea] = useState<string | null>(null);
  const [title, setTitle] = useState<string | null>(null);
  const [image, setImage] = useState<File | null>(null);
  const [formChoice, setFormChoice] = useState<"list" | "idea" | "image" | null>("list");

  const [units, setUnits] = useState<Unit[]>([]);
  const [recipeCategories, setRecipeCategories] = useState<RecipeCategory[]>([]);
  const [newRecipe, setNewRecipe] = useState<Recipe | null>(null);
  const [ingredients, setIngredients] = useState<Ingredient[]>(listeIngredients);

  const ingredientItems = listeIngredients.map((ingredient) => ({
    value: ingredient.name ?? "",
    label: ingredient.name ?? "",
  }));

  const handleFormChoiceChange = (choice: "list" | "idea" | "image") => {
    setFormChoice(choice);
    setListIngredients([]);
    setIdea(null);
    setTitle(null);
    setImage(null);
  };

  const handleAddIngredient = () => {
    if (ingredient && !listIngredients.includes(ingredient)) {
      setListIngredients([...listIngredients, ingredient]);
      setIngredient(null);
      setFormChoice("list");
    }
  };

  const handleRemoveIngredient = (index: number) => {
    const updatedIngredients = listIngredients.filter((_, i) => i !== index);
    setListIngredients(updatedIngredients);
    if (updatedIngredients.length === 0) {
      setFormChoice(null);
    }
  };

  const handleListSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (formChoice === "list" && listIngredients && listIngredients.length != 0) {
      setLoading(true);
      setNewRecipe(null);
      await generateRecipeIngredientsApi(listIngredients)
        .then(async (recipeAI) => {
          await fetchIngredientsApi()
            .then((ingredients) => {
              if (ingredients)
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
        })
        .finally(() => setLoading(false));
    }
  };
  const handleIdeaSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (formChoice === "idea" && idea && idea.trim() != "") {
      setLoading(true);
      setNewRecipe(null);
      await generateRecipeIdeaApi(idea)
        .then(async (recipeAI) => {
          await fetchIngredientsApi()
            .then((ingredients) => {
              if (ingredients)
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
        })
        .finally(() => setLoading(false));
    }
  };
  const handleImageSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (formChoice === "image" && image && title && title.trim() != "") {
      setLoading(true);
      setNewRecipe(null);
      await generateRecipeImageApi(title, image)
        .then(async (recipeAI) => {
          await fetchIngredientsApi()
            .then((ingredients) => {
              if (ingredients)
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
        })
        .finally(() => setLoading(false));
    }
  };

  useEffect(() => {
    fetchRecipeCategoriesApi()
      .then((recipeCategories) => recipeCategories && setRecipeCategories(recipeCategories))
      .catch((error: unknown) => console.error("Erreur de chargement des catégories :", error));

    fetchUnitsApi()
      .then((units) => units && setUnits(units))
      .catch((error: unknown) => console.error("Erreur de chargement des unités :", error));
  }, []);

  return (
    <div className="mx-auto max-w-5xl rounded-lg border-t bg-white p-6 shadow-md">
      <h2 className="mb-6 text-xl font-bold text-gray-800">Générateur de Recette</h2>
      <div className="mb-6">
        <div className="flex gap-4">
          {["list", "idea", "image"].map((choice) => (
            <button
              key={choice}
              onClick={() => handleFormChoiceChange(choice as "list" | "idea" | "image")}
              className={`grow rounded-lg px-6 py-3 text-sm font-medium shadow transition duration-200 ${
                formChoice === choice ? "bg-indigo-600 text-white hover:bg-indigo-700" : "bg-gray-100 text-gray-600 hover:bg-gray-200"
              }`}
            >
              {choice === "list" && "Liste d'ingrédients"}
              {choice === "idea" && "Idée ou Thème"}
              {choice === "image" && "Image"}
            </button>
          ))}
        </div>
      </div>
      {formChoice === "list" && (
        <form onSubmit={(event) => { void handleListSubmit(event); }} className="flex flex-col gap-4">
          <div className="mt-2 flex items-center gap-4">
            <CuisineDataList
              items={ingredientItems}
              selectedValue={ingredient || ""}
              name="ingredient"
              onChange={(value) => setIngredient(value)}
              label="Sélectionner un ingrédient"
              isSearchable={true}
              classNames="flex-grow"
            />
            <button onClick={handleAddIngredient} type="button" className="inline-flex items-center gap-2 rounded bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700">
              <FaPlus />
              Ajouter
            </button>
          </div>
          {listIngredients.length > 0 && (
            <div className="mt-4">
              <h3 className="mb-2 text-lg font-semibold text-gray-700">Ingrédients sélectionnés</h3>
              <ul className="space-y-2">
                {listIngredients.map((ingredient, index) => (
                  <li key={index} className="flex items-center justify-between rounded-md border border-gray-200 bg-white p-2 pr-4 shadow-sm">
                    <span className="pl-4 text-gray-800">{ingredient}</span>
                    <button
                      className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-red-600 hover:bg-red-600 hover:text-white focus:outline-none focus:ring"
                      type="button"
                      onClick={() => handleRemoveIngredient(index)}
                    >
                      <FaRegTrashCan />
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}
          <button
            type="submit"
            className={`mt-4 inline-flex items-center justify-center gap-2 rounded px-6 py-2 font-medium text-white ${loading ? "cursor-not-allowed bg-gray-400" : "bg-green-600 hover:bg-green-700"}`}
            disabled={loading}
          >
            {loading ? <>Création en cours...</> : "Générer une recette"}
          </button>
        </form>
      )}
      {formChoice === "idea" && (
        <form onSubmit={(event) => { void handleIdeaSubmit(event); }} className="flex flex-col gap-4">
          <div className="mt-2">
            <CuisineInputText name="idea" defaultValue={null} onChange={(value) => setIdea(value)} label="Saisir une idée ou un thème" classNames="w-full" />
          </div>
          <button
            type="submit"
            className={`mt-4 inline-flex items-center justify-center gap-2 rounded px-6 py-2 font-medium text-white ${loading ? "cursor-not-allowed bg-gray-400" : "bg-green-600 hover:bg-green-700"}`}
            disabled={loading}
          >
            {loading ? <>Création en cours...</> : "Générer une recette"}
          </button>
        </form>
      )}
      {formChoice === "image" && (
        <form onSubmit={(event) => { void handleImageSubmit(event); }} className="flex flex-col gap-4">
          <div className="mt-2">
            <CuisineInputText name="title" defaultValue={null} onChange={(value) => setTitle(value)} label="Nom de la recette à rechercher" classNames="w-full" />
            <CuisineInputFile name="image" accept="image/*" onChange={(value) => setImage(value)} label="Sélectionner une image" defaultValue={null} />
          </div>
          <button
            type="submit"
            className={`mt-4 inline-flex items-center justify-center gap-2 rounded px-6 py-2 font-medium text-white ${loading ? "cursor-not-allowed bg-gray-400" : "bg-green-600 hover:bg-green-700"}`}
            disabled={loading}
          >
            {loading ? <>Création en cours...</> : "Générer une recette"}
          </button>
        </form>
      )}
      {loading && (
        <>
          <div className="mt-6 flex flex-col items-center justify-center">
            <RiRobot2Line className="mt-10 animate-bounce text-8xl text-green-600" />
          </div>
        </>
      )}
      {newRecipe && (
        <div className="mt-8">
          <h3 className="mb-4 text-lg font-semibold text-gray-800">Nouvelle Recette</h3>
          <RecipeForm recipe={newRecipe} ingredients={ingredients} units={units} recipeCategories={recipeCategories} />
        </div>
      )}
    </div>
  );
}

export default RecipeGeneratorForm;
