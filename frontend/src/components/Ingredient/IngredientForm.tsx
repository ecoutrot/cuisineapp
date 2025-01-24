import { useNavigate } from "react-router-dom";
import { Ingredient } from "../../types/Ingredient";
import { useState } from "react";
import CuisineInputText from "../Forms/CuisineInputText";
import { createIngredientApi, updateIngredientApi } from "../../services/Ingredient";
import CuisineTextarea from "../Forms/CuisineTextarea";

function IngredientForm({ ingredient }: { ingredient: Ingredient | null }) {
  const navigate = useNavigate();

  const [ingredientData, setIngredientData] = useState<Ingredient>(
    ingredient ?? {
      id: null,
      name: "",
      description: "",
    }
  );
  const [saveButtonLabel, setSaveButtonLabel] = useState<string>(ingredient?.id ? "Modifier" : "Créer");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errors, setErrors] = useState<{ name?: string }>({});

  const handleChange = (value: string, name: string) => {
    setIngredientData({ ...ingredientData, [name]: value });
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (!ingredientData.name) {
      setErrors({ name: "Le nom de l'ingredient est obligatoire" });
      return;
    }

    setIsSubmitting(true);
    setErrors({});

    try {
      if (ingredientData.id) {
        setSaveButtonLabel("Enregistrement...");
        await updateIngredientApi(ingredientData).then(() => navigate("/ingredients/"));
      } else {
        setSaveButtonLabel("Enregistrement...");
        await createIngredientApi(ingredientData).then(() => navigate("/ingredients/"));
      }
    } catch (error) {
      console.error("Error saving ingredient:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mx-auto mt-3 max-w-3xl rounded-lg border-t bg-white p-6 font-sans antialiased shadow-md">
      <h1 className="mb-6 text-xl font-semibold text-gray-800">{ingredient?.id ? "Modifier l'ingredient" : "Créer une ingredient"}</h1>
      <form onSubmit={(event) => { void handleSubmit(event); }} className="space-y-6">
        <div>
          <CuisineInputText name="name" defaultValue={ingredientData.name} label="Nom de l'ingredient" onChange={(value) => handleChange(value, "name")} classNames="mt-1" />
          {errors.name && <p className="mt-1 text-xs text-red-500">{errors.name}</p>}
        </div>

        <CuisineTextarea name="name" defaultValue={ingredientData.description} label="Description" onChange={(value) => handleChange(value, "description")} classNames="mt-1" />
        <button
          type="submit"
          disabled={isSubmitting}
          // eslint-disable-next-line tailwindcss/classnames-order
          className={`mt-4 inline-block w-full rounded border px-4 py-2 text-sm font-medium text-white focus:outline-none focus:ring ${
            isSubmitting ? "bg-gray-400 cursor-not-allowed" : "border-indigo-600 bg-indigo-600 hover:bg-indigo-700 focus:ring-indigo-300"
          }`}
        >
          {isSubmitting ? "Enregistrement..." : saveButtonLabel}
        </button>
      </form>
    </div>
  );
}

export default IngredientForm;
