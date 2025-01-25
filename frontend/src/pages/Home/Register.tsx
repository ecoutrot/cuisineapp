import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { fetchForRegister } from "../../services/Auth";
import CuisineInputText from "../../components/Forms/CuisineInputText";
import CuisineInputPassword from "../../components/Forms/CuisineInputPassword";
import Spinner from "../../components/Elements/Spinner";
import { useAuth } from "../../components/Auth/AuthProvider";

function Register() {
  const { setToken } = useAuth();
  const navigate = useNavigate();
  const [username, setUsername] = useState<string>("");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    
    const formData = new FormData(event.currentTarget as HTMLFormElement);
    const formUsername = formData.get("username") as string;
    const formPassword = formData.get("password") as string;

    if (!formUsername || !formPassword) {
      setErrorMessage("Tous les champs sont requis.");
      return;
    }

    const credentials = { username: formUsername, password: formPassword };

    await fetchForRegister(credentials)
      .then(async (token) => {
        if (!token) {
          setErrorMessage("Erreur lors de la connexion. Veuillez réessayer.");
          return;
        }
        setToken(token.accessToken);
        localStorage.setItem("token", token.accessToken);
        await navigate("/");
      })
      .catch((error) => {
        console.error(error);
        setErrorMessage("Erreur lors de l'inscription. Veuillez réessayer.");
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
        <h1 className="mb-6 text-center text-2xl font-bold text-gray-800">Inscription</h1>
        {errorMessage && <div className="mb-4 rounded bg-red-100 p-3 text-sm text-red-700">{errorMessage}</div>}
        <form onSubmit={(event) => { void handleSubmit(event); }} className="mb-2 space-y-6">
          <CuisineInputText name="username" defaultValue={username} placeholder="Indentifiant" onChange={(value) => setUsername(value)} label="Indentifiant" />
          <CuisineInputPassword name="password" placeholder="Mot de passe" label="Mot de passe" />
          <button
            type="submit"
            disabled={isSubmitting}
            className={`w-full rounded-lg bg-indigo-500 px-4 py-2 text-sm font-medium text-white transition-all hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-400 focus:ring-offset-1 ${
              isSubmitting ? "cursor-not-allowed opacity-50" : ""
            }`}
          >
            {isSubmitting ? "Création en cours..." : "Créer un compte"}
          </button>
        </form>
      </div>
    </div>
  );
}

export default Register;
