import { useEffect, useState } from "react";
import { Recipe } from "../../types/Recipe";
import RecipeListCardItem from "../../components/Recipe/RecipeListCardItem";
import { nanoid } from "nanoid";
import { fetchRecipesApi } from "../../services/Recipe";
import { fetchRecipeCategoriesApi } from "../../services/RecipeCategory";
import { RecipeCategory } from "../../types/RecipeCategory";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import { FaMagnifyingGlass } from "react-icons/fa6";
import { BsStars } from "react-icons/bs";
import Spinner from "../../components/Elements/Spinner";
import { Link } from "react-router-dom";

function RecipesView() {
  const limit = 10;

  const [recipes, setRecipes] = useState<Recipe[]>();
  const [recipeCategories, setRecipeCategories] = useState<RecipeCategory[]>([]);
  const [page, setPage] = useState<number>(1);
  const [search, setSearch] = useState<string | null>(null);
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState<string | null>("");

  const handleIncrementPage = (page: number) => {
    if (recipes?.length === limit) {
      setPage(page + 1);
    }
  };
  const handleDecrementPage = (page: number) => {
    if (page > 1) {
      setPage(page - 1);
    }
  };

  const handleDelete = (id: string) => {
    setRecipes(recipes?.filter((recipe) => recipe.id !== id));
  };

  useEffect(() => {
    const handler = setTimeout(() => {
      setPage(1);
      setDebouncedSearchTerm(search);
    }, 500);
    return () => {
      clearTimeout(handler);
    };
  }, [search]);

  useEffect(() => {
    fetchRecipesApi(page, limit, search)
      .then((recipes) => {
        if (!recipes) return;
        setRecipes(recipes);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, [page, limit, debouncedSearchTerm]);

  useEffect(() => {
    fetchRecipeCategoriesApi()
      .then((recipeCategories) => {
        if (!recipeCategories) return
        setRecipeCategories(recipeCategories);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, []);

  if (!recipes) {
    return <Spinner />;
  }

  const listeRecipes = recipes.map((recipe: Recipe) => {
    const recipeCategory = recipeCategories.find((recipeCategory: RecipeCategory) => recipeCategory.id === recipe.recipeCategoryId);
    return <RecipeListCardItem key={nanoid()} recipe={recipe} recipeCategory={recipeCategory ?? null} onDelete={handleDelete} />;
  });

  return (
    <>
      <div className="relative mx-auto mb-6 max-w-3xl rounded-lg border-t bg-white p-6 shadow-md">
        <div className="flex items-center justify-between">
          <div className="relative grow">
            <input
              type="text"
              placeholder="Rechercher une recette"
              className="w-full rounded-md border-gray-200 p-2.5 pe-10 shadow-sm sm:text-sm"
              value={search ?? ""}
              onChange={(e) => setSearch(e.target.value)}
            />
            <span className="absolute inset-y-0 right-4 flex items-center">
              <div className="text-gray-600 hover:text-gray-700">
                <FaMagnifyingGlass />
              </div>
            </span>
          </div>
          <Link
            to="/recipes/generator"
            className="group ml-4 rounded-lg border border-green-600 bg-green-600 p-2.5 px-5 text-sm font-medium text-white shadow hover:bg-transparent hover:text-green-600 focus:outline-none focus:ring-2 focus:ring-green-600 active:text-green-500"
          >
            <div className="relative flex flex-col items-center">
              <BsStars className="text-xl" />
              <span className="absolute top-7 z-10 w-32 scale-0 text-center text-base text-green-600 group-hover:scale-100">Générer une recette</span>
            </div>
          </Link>
        </div>
        {listeRecipes.length > 0 && (
          <>
            <div className="relative mt-6 flex flex-col">
              <nav className="flex min-w-[240px] flex-col gap-1">{listeRecipes}</nav>
            </div>
            <ol className="my-6 flex justify-center gap-2 text-lg font-medium">
              <li>
                <button
                  onClick={() => handleDecrementPage(page)}
                  className={`inline-flex size-10 items-center justify-center rounded-full border border-gray-200 bg-white ${
                    page > 1 ? "text-gray-900 hover:bg-gray-100" : "cursor-not-allowed text-gray-300"
                  }`}
                  disabled={page <= 1}
                >
                  <FaChevronLeft />
                </button>
              </li>

              <li className="inline-flex size-10 items-center justify-center rounded-full border border-indigo-600 bg-indigo-600 text-white">{page}</li>

              <li>
                <button
                  onClick={() => handleIncrementPage(page)}
                  className={`inline-flex size-10 items-center justify-center rounded-full border border-gray-200 bg-white ${
                    recipes?.length === limit ? "text-gray-900 hover:bg-gray-100" : "cursor-not-allowed text-gray-300"
                  }`}
                  disabled={recipes?.length !== limit}
                >
                  <FaChevronRight />
                </button>
              </li>
            </ol>
          </>
        )}
      </div>
    </>
  );
}

export default RecipesView;
