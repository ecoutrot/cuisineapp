import { useNavigate } from "react-router-dom";
import { Unit } from "../../types/Unit";
import { useState } from "react";
import CuisineInputText from "../Forms/CuisineInputText";
import { createUnitApi, updateUnitApi } from "../../services/Unit";

function UnitForm({ unit }: { unit: Unit | null }) {
  const navigate = useNavigate();

  const [unitData, setUnitData] = useState<Unit>(
    unit ?? {
      id: null,
      name: "",
    }
  );
  const [saveButtonLabel, setSaveButtonLabel] = useState<string>(unit?.id ? "Modifier" : "Créer");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errors, setErrors] = useState<{ name?: string }>({});

  const handleChange = (value: string, name: string) => {
    setUnitData({ ...unitData, [name]: value });
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (!unitData.name) {
      setErrors({ name: "Le nom de l'unité est obligatoire" });
      return;
    }

    setIsSubmitting(true);
    setErrors({});

    try {
      if (unitData.id) {
        setSaveButtonLabel("Enregistrement...");
        await updateUnitApi(unitData).then(() => navigate("/units/"));
      } else {
        setSaveButtonLabel("Enregistrement...");
        await createUnitApi(unitData).then(() => navigate("/units/"));
      }
    } catch (error) {
      console.error("Error saving unit:", error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mx-auto mt-3 max-w-3xl rounded-lg border-t bg-white p-6 font-sans antialiased shadow-md">
      <h1 className="mb-6 text-xl font-semibold text-gray-800">{unit?.id ? "Modifier l'unité" : "Créer une unité"}</h1>
      <form onSubmit={(event) => { void handleSubmit(event); }} className="space-y-6">
        <div>
          <CuisineInputText name="name" defaultValue={unitData.name} label="Nom de l'unité" onChange={(value) => handleChange(value, "name")} classNames="mt-1" />
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

export default UnitForm;
