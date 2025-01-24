import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { fetchForAccessToken } from "../../services/Auth";
import CuisineInputText from "../../components/Forms/CuisineInputText";
import CuisineInputPassword from "../../components/Forms/CuisineInputPassword";
import { useAuth } from "../../components/Auth/AuthProvider";

function Login() {
  const { setToken } = useAuth();
  const navigate = useNavigate();
  const [username, setUsername] = useState<string>("");
  const [isSubmitting, setIsSubmitting] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setIsSubmitting(true);
    setErrorMessage(null);

    const formData = new FormData(event.currentTarget);
    const formUsername = formData.get("username") as string;
    const formPassword = formData.get("password") as string;

    if (!formUsername || !formPassword) {
      setErrorMessage("Tous les champs sont requis.");
      setIsSubmitting(false);
      return;
    }

    const credentials = { username: formUsername, password: formPassword };

    await fetchForAccessToken(credentials)
      .then(async (token) => {
        if (!token) {
          setErrorMessage("Erreur lors de la connexion. Veuillez réessayer.");
          setIsSubmitting(false);
          return;
        }
        setToken(token);
        localStorage.setItem("token", token);
        await navigate("/");
      })
      .catch((error) => {
        console.error(error);
        setErrorMessage("Identifiants incorrects. Veuillez réessayer.");
        setIsSubmitting(false);
      });
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="w-full max-w-md rounded-lg bg-white p-6 shadow-lg sm:p-8">
        <h1 className="mb-6 text-center text-2xl font-bold text-gray-800">Connexion</h1>
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
            {isSubmitting ? "Connexion en cours..." : "Se connecter"}
          </button>
        </form>
      </div>
    </div>
  );
}

export default Login;
