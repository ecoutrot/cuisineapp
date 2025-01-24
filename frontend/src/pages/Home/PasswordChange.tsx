import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { fetchForChangePassword } from "../../services/Auth";
import CuisineInputPassword from "../../components/Forms/CuisineInputPassword";
import Spinner from "../../components/Elements/Spinner";
import { useAuth } from "../../components/Auth/AuthProvider";

function PasswordChange() {
  const { setToken } = useAuth();
  const navigate = useNavigate();
  const [oldPassword, setOldPassword] = useState<string>("");
  const [newPassword, setNewPassword] = useState<string>("");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    setIsSubmitting(true);
    setErrorMessage(null);

    await fetchForChangePassword({ oldPassword, newPassword })
      .then(async (token) => {
        if (!token) {
          setErrorMessage("Erreur lors de la connexion. Veuillez réessayer.");
          return;
        }
        setToken(token);
        localStorage.setItem("token", token);
        await navigate("/");
      })
      .catch((error) => {
        console.error(error);
        setErrorMessage("Une erreur s'est produite. Veuillez réessayer.");
      })
      .finally(() => {
        setIsSubmitting(false);
      });
  };

  return isSubmitting ? (
    <Spinner />
  ) : (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="w-full max-w-md rounded-lg bg-white p-6 shadow-lg sm:p-8">
        <h1 className="mb-6 text-center text-2xl font-bold text-gray-800">Changer mon mot de passe</h1>
        {errorMessage && <div className="mb-4 rounded bg-red-100 p-3 text-sm text-red-700">{errorMessage}</div>}
        <form onSubmit={(event) => { void handleSubmit(event); }} className="mb-2 space-y-6">
          <CuisineInputPassword name="oldPassword" placeholder="Ancien mot de passe" onChange={(value) => setOldPassword(value)} label="Ancien mot de passe" />
          <CuisineInputPassword name="newPassword" placeholder="Nouveau mot de passe" onChange={(value) => setNewPassword(value)} label="Nouveau mot de passe" />
          <button
            type="submit"
            disabled={isSubmitting}
            className={`w-full rounded-lg bg-indigo-500 px-4 py-2 text-sm font-medium text-white transition-all hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-1 ${
              isSubmitting ? "cursor-not-allowed opacity-50" : ""
            }`}
          >
            {isSubmitting ? "Changement en cours..." : "Valider"}
          </button>
        </form>
      </div>
    </div>
  );
}

export default PasswordChange;
