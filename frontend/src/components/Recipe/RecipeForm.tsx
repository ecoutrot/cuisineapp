import { Link, useNavigate } from "react-router-dom";
import { createRecipeApi, updateRecipeApi } from "../../services/Recipe";
import { Recipe } from "../../types/Recipe";
import { Ingredient } from "../../types/Ingredient";
import { Unit } from "../../types/Unit";
import CuisineInputNumber from "../Forms/CuisineInputNumber";
import CuisineDataList from "../Forms/CuisineDataList";
import { useState } from "react";
import { RecipeIngredient } from "../../types/RecipeIngredient";
import { nanoid } from "nanoid";
import CuisineTextarea from "../Forms/CuisineTextarea";
import CuisineInputText from "../Forms/CuisineInputText";
import CuisineSelect from "../Forms/CuisineSelect";
import { toFloatOrNull, toIntOrNull, toStringOrNull } from "../../helpers/Parsers";
import { RecipeCategory } from "../../types/RecipeCategory";
import { BsStars } from "react-icons/bs";
import { FaRegTrashCan } from "react-icons/fa6";

function RecipeForm({ recipe, ingredients, units, recipeCategories }: { recipe: Recipe | null; ingredients: Ingredient[]; units: Unit[]; recipeCategories: RecipeCategory[] }) {
  if (recipe === null) {
    recipe = {
      id: null,
      name: null,
      description: null,
      recipeIngredients: null,
      steps: null,
      recipeCategoryId: null,
      preparationTime: null,
      cookingTime: null,
      restTime: null,
      portions: null,
      difficulty: null,
      price: null,
      cookingType: null,
      calories: null,
      advice: null,
      userId: null,
    };
  }
  const navigate = useNavigate();
  const [name, setName] = useState<string | null>(recipe?.name);
  const [description, setDescription] = useState<string | null>(recipe?.description);
  const [recipeCategoryId, setRecipeCategoryId] = useState<string | null>(recipe?.recipeCategoryId);
  const [preparationTime, setPreparationTime] = useState<number | null>(recipe?.preparationTime);
  const [cookingTime, setCookingTime] = useState<number | null>(recipe?.cookingTime);
  const [restTime, setRestTime] = useState<number | null>(recipe?.restTime);
  const [portions, setPortions] = useState<number | null>(recipe?.portions);
  const [difficulty, setDifficulty] = useState<number | null>(recipe?.difficulty);
  const [price, setPrice] = useState<number | null>(recipe?.price);
  const [cookingType, setCookingType] = useState<number | null>(recipe?.cookingType);
  const [calories, setCalories] = useState<number | null>(recipe?.calories);
  const [advice, setAdvice] = useState<string | null>(recipe?.advice ?? "");
  const [saveButtonLabel, setSaveButtonLabel] = useState<string>(recipe?.id != null ? "Modifier" : "Créer");
  const [recipeIngredients, setRecipeIngredients] = useState<RecipeIngredient[]>(recipe.recipeIngredients ?? [{ id: null, unitId: null, quantity: null, ingredientId: null, optional: false }]);
  const [steps, setSteps] = useState<string[]>((recipe.steps?.length == 0 ? null : recipe.steps) ?? [""]);
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errors, setErrors] = useState<{ name?: string }>({});

  const ingredientItems = ingredients.map((ingredient) => {
    return { value: ingredient.id ?? "", label: ingredient.name ?? "" };
  });
  const uniteItems = units.map((unit) => {
    return { value: unit.id ?? "", label: unit.name ?? "" };
  });
  uniteItems.unshift({ value: "", label: "" });
  const recipeCategoryItems = recipeCategories.map((recipeCategory) => {
    return { value: recipeCategory.id ?? "", label: recipeCategory.name ?? "" };
  });
  const cookingTypes = [
    { value: "0", label: "Pas de cuisson" },
    { value: "1", label: "Four" },
    { value: "2", label: "Plaque" },
    { value: "3", label: "Vapeur" },
    { value: "4", label: "Friteuse" },
    { value: "5", label: "Barbecue" },
    { value: "6", label: "Autre" },
  ];
  const difficultyTypes = [
    { value: "0", label: "Facile" },
    { value: "1", label: "Moyen" },
    { value: "2", label: "Difficile" },
  ];
  const PrixTypes = [
    { value: "0", label: "€" },
    { value: "1", label: "€€" },
    { value: "2", label: "€€€" },
  ];

  const handleIngredientChange = (index: number, field: keyof RecipeIngredient, value: string | number | boolean | null) => {
    const updatedIngredients = [...recipeIngredients];
    updatedIngredients[index] = {
      ...updatedIngredients[index],
      [field]: value,
    };
    setRecipeIngredients(updatedIngredients);
  };

  const handleAddRecipeIngredient = () => {
    setRecipeIngredients([...recipeIngredients, { id: null, unitId: null, quantity: null, ingredientId: null, optional: false }]);
  };

  const handleRemoveRecipeIngredient = (index: number) => {
    setRecipeIngredients(recipeIngredients.filter((_, i) => i !== index));
  };

  const handleStepChange = (index: number, value: string) => {
    const updatedSteps = [...steps];
    updatedSteps[index] = value;
    setSteps(updatedSteps);
  };

  const handleAddStep = () => {
    setSteps([...steps, ""]);
  };

  const handleRemoveStep = (index: number) => {
    setSteps(steps.filter((_, i) => i !== index));
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget as HTMLFormElement);
    if (formData.get("name") === null) {
      setErrors({ name: "Le nom de la recette est obligatoire" });
      return;
    }
    const arrayRecipeIngredients = [];
    if (recipeIngredients != null) {
      for (let i = 0; i < recipeIngredients.length; i++) {
        const id = toStringOrNull(formData.get(`rIngredients[${i}].id`) as string);
        const unitId = toStringOrNull(formData.get(`rIngredients[${i}].unitId`) as string);
        const quantity = toFloatOrNull(formData.get(`rIngredients[${i}].quantity`) as string);
        const ingredientId = toStringOrNull(formData.get(`rIngredients[${i}].ingredientId`) as string);
        const optional = recipeIngredients[i].optional;
        if (id || unitId || quantity || ingredientId) {
          arrayRecipeIngredients.push({ id, unitId, quantity, ingredientId, optional });
        }
      }
    }
    const arraySteps = [];
    if (steps != null) {
      for (let i = 0; i < steps.length; i++) {
        const step = toStringOrNull(formData.get(`steps[${i}]`) as string);
        if (step) {
          arraySteps.push(step);
        }
      }
    }
    const recipeEdited: Recipe = {
      ...recipe,
      name: toStringOrNull(formData.get("name") as string),
      description: toStringOrNull(formData.get("description") as string),
      preparationTime: toIntOrNull(formData.get("preparationTime") as string),
      recipeIngredients: arrayRecipeIngredients,
      steps: arraySteps,
      recipeCategoryId: toStringOrNull(formData.get("recipeCategoryId") as string),
      cookingTime: toIntOrNull(formData.get("cookingTime") as string),
      restTime: toIntOrNull(formData.get("restTime") as string),
      portions: toIntOrNull(formData.get("portions") as string),
      difficulty: toIntOrNull(formData.get("difficulty") as string),
      price: toIntOrNull(formData.get("price") as string),
      cookingType: toIntOrNull(formData.get("cookingType") as string),
      calories: toIntOrNull(formData.get("calories") as string),
      advice: toStringOrNull(formData.get("advice") as string),
      userId: toStringOrNull(formData.get("userId") as string),
    };

    setIsSubmitting(true);
    setErrors({});

    try {
      if (recipe?.id) {
        setSaveButtonLabel("Enregistrement...");
        await updateRecipeApi(recipeEdited).then(() => navigate("/recipes/"));
      } else {
        setSaveButtonLabel("Enregistrement...");
        await createRecipeApi(recipeEdited).then(() => navigate("/recipes/"));
      }
    } catch (error) {
      setSaveButtonLabel("Erreur");
      console.error("Error saving recipe:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <>
      <form onSubmit={(event) => { void handleSubmit(event); }} className="mx-auto max-w-5xl p-6 font-sans antialiased">
        <div className="flex items-center justify-between">
          <CuisineInputText
            defaultValue={name ?? ""}
            name="name"
            label="Nom de la recette"
            onChange={(value: string) => {
              setName(value);
            }}
            classNames="w-full"
          />
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
        {errors.name && <p className="mt-1 text-xs text-red-500">{errors.name}</p>}
        <CuisineTextarea
          defaultValue={description ?? ""}
          name="description"
          label="Description"
          onChange={(value: string) => {
            setDescription(value);
          }}
        />
        <hr className="m-4 mt-8" />
        <CuisineDataList
          items={recipeCategoryItems}
          selectedValue={recipeCategoryId ?? ""}
          name="recipeCategoryId"
          label="Catégorie"
          placeholder="- - - - - - - - - "
          isSearchable={false}
          onChange={(value: string | null) => {
            setRecipeCategoryId(value);
          }}
        />
        <hr className="m-4 mt-8" />
        {recipeIngredients.map((recipeIngredient, index) => (
          <div key={nanoid()} className="gap-4 md:flex">
            <div className="flex gap-4">
              <input type="hidden" name={`rIngredients[${index}].id`} defaultValue={recipeIngredient.id ?? ""} />
              <CuisineInputNumber
                defaultValue={recipeIngredient.quantity ?? 0}
                name={`rIngredients[${index}].quantity`}
                onChange={(value) => handleIngredientChange(index, "quantity", value)}
                label="Quantité"
                classNames={"w-full md:w-auto"}
              />
              <CuisineSelect
                items={uniteItems}
                selectedValue={recipeIngredient.unitId ?? ""}
                name={`rIngredients[${index}].unitId`}
                onChange={(value) => handleIngredientChange(index, "unitId", value)}
                label="Unité"
                placeholder="- - - -"
                classNames={"w-full md:w-auto"}
              />
            </div>
            <div className="flex w-full gap-4">
              <CuisineDataList
                items={ingredientItems}
                selectedValue={recipeIngredient.ingredientId ?? ""}
                name={`rIngredients[${index}].ingredientId`}
                onChange={(value) => handleIngredientChange(index, "ingredientId", value)}
                label="Ingrédient"
                placeholder="- - - - - - - - - - -"
                isSearchable={true}
                classNames={"w-full"}
              />
              <div className="flex items-center">
                <button
                  type="button"
                  onClick={() => handleIngredientChange(index, "optional", !recipeIngredient.optional)}
                  className={`flex size-6 items-center justify-center rounded-full border ${recipeIngredient.optional ? "bg-orange-300 text-white" : "bg-green-400 text-white"}`}
                  title="Rendre cet ingrédient facultatif"
                >
                  {recipeIngredient.optional ? "?" : "✓"}
                </button>
              </div>
              <div className="pt-4">
                <button
                  className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-red-600 hover:bg-red-600 hover:text-white focus:outline-none focus:ring"
                  type="button"
                  onClick={() => handleRemoveRecipeIngredient(index)}
                >
                  <FaRegTrashCan />
                </button>
              </div>
            </div>
          </div>
        ))}
        <button
          type="button"
          onClick={handleAddRecipeIngredient}
          className="mb-4 inline-block rounded border border-green-600 bg-green-600 px-6 py-2 text-sm font-medium text-white hover:bg-transparent hover:text-green-600 focus:outline-none focus:ring active:text-green-500"
        >
          Ajouter un ingrédient
        </button>
        <hr className="m-4" />
        {steps.map((step, index) => (
          <div key={nanoid()} className="flex gap-4">
            <CuisineTextarea defaultValue={step ?? ""} name={`steps[${index}]`} onChange={(value) => handleStepChange(index, value)} label={`Etape n°${index + 1}`} classNames="w-full" />
            <div className="pt-4">
              <button
                className="flex size-10 items-center justify-center rounded-full bg-gray-100 text-red-600 hover:bg-red-600 hover:text-white focus:outline-none focus:ring"
                type="button"
                onClick={() => handleRemoveStep(index)}
              >
                <FaRegTrashCan />
              </button>
            </div>
          </div>
        ))}
        <button
          type="button"
          onClick={handleAddStep}
          className="mb-4 inline-block rounded border border-green-600 bg-green-600 px-6 py-2 text-sm font-medium text-white hover:bg-transparent hover:text-green-600 focus:outline-none focus:ring active:text-green-500"
        >
          Ajouter une étape
        </button>
        <hr className="m-4 mb-8" />
        <CuisineDataList
          items={cookingTypes}
          selectedValue={cookingType?.toString() ?? ""}
          name="cookingType"
          label="Type de cuisson"
          placeholder="- - - - - - - - - "
          isSearchable={false}
          onChange={(value: string | null) => {
            setCookingType(toIntOrNull(value));
          }}
        />
        <div className="flex gap-4">
          <CuisineInputNumber
            defaultValue={preparationTime ?? 0}
            name="preparationTime"
            label="Temps de préparation"
            classNames={"w-full"}
            onChange={(value: number) => {
              setPreparationTime(value);
            }}
          />
          <CuisineInputNumber
            defaultValue={cookingTime ?? 0}
            name="cookingTime"
            label="Temps de cuisson"
            classNames={"w-full"}
            onChange={(value: number) => {
              setCookingTime(value);
            }}
          />
          <CuisineInputNumber
            defaultValue={restTime ?? 0}
            name="restTime"
            label="Temps de repos"
            classNames={"w-full"}
            onChange={(value: number) => {
              setRestTime(value);
            }}
          />
        </div>
        <div className="flex gap-4">
          <CuisineInputNumber
            defaultValue={portions ?? 0}
            name="portions"
            label="Nombre de portions"
            classNames={"w-full"}
            onChange={(value: number) => {
              setPortions(value);
            }}
          />
          <CuisineDataList
            items={difficultyTypes}
            selectedValue={difficulty?.toString() ?? ""}
            name="difficulty"
            label="Difficulté"
            placeholder=""
            classNames={"w-full"}
            isSearchable={false}
            onChange={(value: string | null) => {
              setDifficulty(toIntOrNull(value));
            }}
          />
          <CuisineDataList
            items={PrixTypes}
            selectedValue={price?.toString() ?? ""}
            name="price"
            label="Prix"
            placeholder=""
            classNames={"w-full"}
            isSearchable={false}
            onChange={(value: string | null) => {
              setPrice(toIntOrNull(value));
            }}
          />
          <CuisineInputNumber
            defaultValue={calories ?? 0}
            name="calories"
            label="Calories"
            classNames={"w-full"}
            onChange={(value: number) => {
              setCalories(value);
            }}
          />
        </div>
        <hr className="m-4 mb-8" />
        <CuisineTextarea
          defaultValue={advice ?? ""}
          name="advice"
          label="Conseils"
          onChange={(value: string) => {
            setAdvice(value);
          }}
        />
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
    </>
  );
}

export default RecipeForm;
