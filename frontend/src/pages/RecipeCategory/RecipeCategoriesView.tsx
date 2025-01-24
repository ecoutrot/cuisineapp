import { useEffect, useState } from "react";
import { RecipeCategory } from "../../types/RecipeCategory";
import RecipeCategoryListCardItem from "../../components/RecipeCategory/RecipeCategoryListCardItem";
import { nanoid } from "nanoid";
import { fetchRecipeCategoriesApi } from "../../services/RecipeCategory";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";
import Spinner from "../../components/Elements/Spinner";

function RecipeCategoriesView() {
  const limit = 10;

  const [recipeCategories, setRecipeCategories] = useState<RecipeCategory[]>();
  const [page, setPage] = useState<number>(1);

  const handleIncrementPage = (page: number) => {
    if (recipeCategories?.length === limit) {
      setPage(page + 1);
    }
  };
  const handleDecrementPage = (page: number) => {
    if (page > 1) {
      setPage(page - 1);
    }
  };

  const handleDelete = (id: string) => {
    setRecipeCategories(recipeCategories?.filter((recipeCategory) => recipeCategory.id !== id));
  };

  useEffect(() => {
    fetchRecipeCategoriesApi(page, limit)
      .then((recipeCategories) => {
        if (!recipeCategories) return
        setRecipeCategories(recipeCategories);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, [page, limit]);

  if (!recipeCategories) {
    return <Spinner />;
  }

  const listeRecipeCategories = recipeCategories.map((recipeCategory: RecipeCategory) => {
    return <RecipeCategoryListCardItem key={nanoid()} recipeCategory={recipeCategory} onDelete={handleDelete} />;
  });

  return (
    <>
      <div className="relative mx-auto mb-6 max-w-3xl rounded-lg border-t bg-white p-6 shadow-md">
        {listeRecipeCategories.length > 0 && (
          <>
            <div className="relative mt-6 flex flex-col">
              <nav className="flex min-w-[240px] flex-col gap-1">{listeRecipeCategories}</nav>
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
                    recipeCategories?.length === limit ? "text-gray-900 hover:bg-gray-100" : "cursor-not-allowed text-gray-300"
                  }`}
                  disabled={recipeCategories?.length !== limit}
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

export default RecipeCategoriesView;
