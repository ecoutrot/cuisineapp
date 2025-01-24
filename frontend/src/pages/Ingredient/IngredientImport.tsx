import { useState } from "react";
import { createIngredientApi, importIngredientsApi } from "../../services/Ingredient";

const IngredientImport = () => {
  const [urlTreated, setUrlTreated] = useState<string>("");
  const [currentCount, setCurrentCount] = useState<number>(0);
  const [message, setMessage] = useState<string>("");
  const [progress, setProgress] = useState<number>(0);
  const [isImporting, setIsImporting] = useState<boolean>(false);

  const alphabet = "abcdefghijklmnopqrstuvwxyz";

  const handleImport = async () => {
    setIsImporting(true);
    setProgress(0);
    setMessage("");
    setCurrentCount(0);

    try {
      const totalSteps = alphabet.length * 30;
      for (const letter of alphabet) {
        for (let i = 0; i < 30; i++) {
          setUrlTreated(`${letter}/${i}`);
          try {
            const newIngredients = await importIngredientsApi(letter, i);
            if (newIngredients) {
              for (const ingredient of newIngredients) {
                await createIngredientApi(ingredient);
                setCurrentCount((prev) => prev + 1);
              }
            }
          } catch (error: unknown) {
            if (error instanceof Error && error.message.includes("404")) {
              i = 30;
            } else {
              console.error(`Erreur lors de l'importation ${letter}/${i}:`, error);
              setMessage(`Erreur sur ${letter}/${i}. Importation poursuivie...`);
            }
          } finally {
            const completedSteps = alphabet.indexOf(letter) * 30 + i + 1;
            setProgress(Math.round((completedSteps / totalSteps) * 100));
          }
        }
      }

      setMessage("Importation terminée avec succès.");
    } catch (error: unknown) {
      setMessage(`Une erreur globale s'est produite : ${error?.toString()}`);
    } finally {
      setIsImporting(false);
    }
  };

  return (
    <div className="mx-auto max-w-3xl rounded-lg border-t bg-white p-6 shadow-md">
      <h3 className="mb-4 text-lg font-semibold text-gray-800">Importer des ingrédients</h3>

      <div className="mb-4">
        <div className="relative h-4 w-full overflow-hidden rounded-full bg-gray-200">
          <div
            className="absolute left-0 top-0 h-full bg-indigo-600 transition-all duration-300"
            style={{ width: `${progress}%` }}
            role="progressbar"
            aria-valuenow={progress}
            aria-valuemin={0}
            aria-valuemax={100}
          ></div>
        </div>
        <p className="mt-2 text-sm text-gray-500">
          Progression : {progress}% | Traitement de l'URL : {urlTreated ?? "x/x"}.
        </p>
      </div>

      <div className="mb-4">
        <p className="text-sm text-gray-700">{currentCount} ingrédients insérés avec succès.</p>
        {message && <p className="mt-1 text-sm text-gray-600">{message}</p>}
      </div>

      <button
        className={`w-full rounded-lg px-4 py-2 font-semibold text-white transition ${
          isImporting ? "cursor-not-allowed bg-gray-400" : "bg-indigo-600 hover:bg-indigo-700 focus:ring focus:ring-indigo-400"
        }`}
        onClick={() => void handleImport()}
        disabled={isImporting}
      >
        {isImporting ? (
          <div className="flex items-center justify-center gap-2">
            <span className="size-5 animate-spin rounded-full border-y-2 border-white"></span>
            Importation en cours...
          </div>
        ) : (
          "Importer"
        )}
      </button>
    </div>
  );
};

export default IngredientImport;
