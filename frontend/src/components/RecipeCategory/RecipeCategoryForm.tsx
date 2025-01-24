import { useNavigate } from "react-router-dom";
import { RecipeCategory } from "../../types/RecipeCategory";
import { useState } from "react";
import CuisineInputText from "../Forms/CuisineInputText";
import { createRecipeCategoryApi, updateRecipeCategoryApi } from "../../services/RecipeCategory";

function RecipeCategoryForm({ recipeCategory }: { recipeCategory: RecipeCategory | null }) {
  const navigate = useNavigate();

  const [recipeCategoryData, setRecipeCategoryData] = useState<RecipeCategory>(
    recipeCategory ?? {
      id: null,
      name: "",
    }
  );
  const [saveButtonLabel, setSaveButtonLabel] = useState<string>(recipeCategory?.id ? "Modifier" : "Créer");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errors, setErrors] = useState<{ name?: string }>({});

  const handleChange = (value: string, name: string) => {
    setRecipeCategoryData({ ...recipeCategoryData, [name]: value });
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!recipeCategoryData.name) {
      setErrors({ name: "Le nom de la catégorie est obligatoire" });
      return;
    }

    setIsSubmitting(true);
    setErrors({});

    try {
      if (recipeCategoryData.id) {
        setSaveButtonLabel("Enregistrement...");
        await updateRecipeCategoryApi(recipeCategoryData).then(() => navigate("/recipeCategories/"));
      } else {
        setSaveButtonLabel("Enregistrement...");
        await createRecipeCategoryApi(recipeCategoryData).then(() => navigate("/recipeCategories/"));
      }
    } catch (error) {
      console.error("Error saving recipeCategory:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mx-auto mt-3 max-w-3xl rounded-lg border-t bg-white p-6 font-sans antialiased shadow-md">
      <h1 className="mb-6 text-xl font-semibold text-gray-800">{recipeCategory?.id ? "Modifier la catégorie" : "Créer une catégorie"}</h1>
      <form onSubmit={(event) => { void handleSubmit(event); }} className="space-y-6">
        <div>
          <CuisineInputText name="name" defaultValue={recipeCategoryData.name} label="Nom de la catégorie" onChange={(value) => handleChange(value, "name")} classNames="mt-1" />
          {errors.name && <p className="mt-1 text-xs text-red-500">{errors.name}</p>}
        </div>
        <button
          type="submit"
          disabled={isSubmitting}
          className={`mt-4 inline-block w-full rounded border px-4 py-2 text-sm font-medium text-white focus:outline-none focus:ring ${
            isSubmitting ? "cursor-not-allowed bg-gray-400" : "border-indigo-600 bg-indigo-600 hover:bg-indigo-700 focus:ring-indigo-300"
          }`}
        >
          {isSubmitting ? "Enregistrement..." : saveButtonLabel}
        </button>
      </form>
    </div>
  );
}

export default RecipeCategoryForm;
