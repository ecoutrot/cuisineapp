import { useEffect, useState } from "react";
import { Ingredient } from "../../types/Ingredient";
import IngredientListCardItem from "../../components/Ingredient/IngredientListCardItem";
import { nanoid } from "nanoid";
import { fetchIngredientsApi } from "../../services/Ingredient";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import { FaMagnifyingGlass } from "react-icons/fa6";
import Spinner from "../../components/Elements/Spinner";

function IngredientsView() {
  const limit = 10;

  const [ingredients, setIngredients] = useState<Ingredient[]>();
  const [page, setPage] = useState<number>(1);
  const [search, setSearch] = useState<string | null>(null);
  const [searchWithDelay, setSearchWithDelay] = useState<string | null>("");

  const handleIncrementPage = (page: number) => {
    if (ingredients?.length === limit) {
      setPage(page + 1);
    }
  };
  const handleDecrementPage = (page: number) => {
    if (page > 1) {
      setPage(page - 1);
    }
  };

  const handleDelete = (id: string) => {
    setIngredients(ingredients?.filter((ingredient) => ingredient.id !== id));
  };

  useEffect(() => {
    const handler = setTimeout(() => {
      setPage(1);
      setSearchWithDelay(search);
    }, 500);
    return () => {
      clearTimeout(handler);
    };
  }, [search]);

  useEffect(() => {
    fetchIngredientsApi(page, limit, search)
      .then((ingredients) => {
        if (!ingredients) return;
        setIngredients(ingredients);
      })
      .catch((error: unknown) => {
        console.error(error);
      });

    // TODO fetchCategory
  }, [page, limit, searchWithDelay]);

  if (!ingredients) {
    return <Spinner />;
  }

  const listeIngredients = ingredients.map((ingredient: Ingredient) => {
    // TODO : add category
    const ingredientCategory = "";
    return <IngredientListCardItem key={nanoid()} ingredient={ingredient} ingredientCategory={ingredientCategory} onDelete={handleDelete} />;
  });

  return (
    <>
      <div className="relative mx-auto mb-6 max-w-3xl rounded-lg border-t bg-white p-6 shadow-md">
        <div className="flex items-center justify-between">
          <div className="relative grow">
            <input
              type="text"
              placeholder="Rechercher un ingrédient"
              className="w-full rounded-md border-gray-200 p-2.5 pe-10 shadow-sm sm:text-sm"
              value={search ?? ""}
              onChange={(e) => setSearch(e.target.value)}
            />
            <span className="absolute inset-y-0 end-1 grid w-10 place-content-center">
              <button type="button" className="text-gray-600 hover:text-gray-700">
                <FaMagnifyingGlass />
              </button>
            </span>
          </div>
        </div>
        {listeIngredients.length > 0 && (
          <>
            <div className="relative mt-6 flex flex-col">
              <nav className="flex min-w-[240px] flex-col gap-1">{listeIngredients}</nav>
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
                    ingredients?.length === limit ? "text-gray-900 hover:bg-gray-100" : "cursor-not-allowed text-gray-300"
                  }`}
                  disabled={ingredients?.length !== limit}
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

export default IngredientsView;
