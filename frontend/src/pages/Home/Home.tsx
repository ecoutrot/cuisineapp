import { FaShareSquare, FaUserCheck } from "react-icons/fa";
import { FaKitchenSet, FaMagnifyingGlass } from "react-icons/fa6";
import { Link } from "react-router-dom";

function Home() {
  return (
    <div className="bg-gray-50">
      <section className="relative bg-indigo-600 text-white">
        <div className="container mx-auto flex flex-col items-center justify-between px-6 py-16 lg:flex-row lg:px-12">
          <div className="max-w-xl text-center lg:text-left">
            <h1 className="mb-6 text-4xl font-bold lg:text-5xl">
              Bienvenue sur <span className="text-gray-200">Livre de cuisine</span>
            </h1>
            <p className="mb-6 text-lg">
              Transformer vos idées culinaires en plats délicieux. Gagnez du temps, organisez vos recettes et inspirez-vous avec une bibliothèque des recettes des autres. Livre de cuisine est une
              plateforme tout-en-un pour les amateurs de cuisine.
            </p>
            <div className="flex flex-col gap-4 sm:flex-row sm:justify-center lg:justify-start">
              <Link to="/recipes" className="rounded bg-white px-6 py-3 text-lg font-medium text-indigo-900 hover:bg-white">
                Explorer les Recettes
              </Link>
              <Link to="/recipes/create" className="rounded border border-white px-6 py-3 text-lg font-medium text-white hover:bg-white hover:text-indigo-900">
                Créer une recette
              </Link>
            </div>
          </div>
          <div className="mt-10 max-w-lg lg:mt-0 lg:max-w-none">
            <FaKitchenSet className="size-52" />
          </div>
        </div>
      </section>
      <section className="bg-gray-100 px-6 py-16">
        <div className="container mx-auto">
          <h2 className="mb-6 text-center text-3xl font-bold text-gray-800">
            Comment utiliser <span className="text-indigo-600">Livre de cuisine</span> ?
          </h2>
          <div className="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-3">
            <div className="rounded-lg bg-white p-6 text-center shadow-md">
              <FaUserCheck className="mx-auto mb-4 size-10 h-16" />
              <h3 className="mb-2 text-xl font-semibold text-gray-800">1. Inscription</h3>
              <p className="text-gray-600">Obtenez un compte gratuitement pour commencer votre aventure culinaire.</p>
            </div>
            <div className="rounded-lg bg-white p-6 text-center shadow-md">
              <FaMagnifyingGlass className="mx-auto mb-4 size-10 h-16" />
              <h3 className="mb-2 text-xl font-semibold text-gray-800">2. Explorez les Recettes</h3>
              <p className="text-gray-600">Trouvez l'inspiration parmi nos nombreuses recettes classifiées.</p>
            </div>
            <div className="rounded-lg bg-white p-6 text-center shadow-md">
              <FaShareSquare className="mx-auto mb-4 size-10 h-16" />
              <h3 className="mb-2 text-xl font-semibold text-gray-800">3. Partagez vos Idées</h3>
              <p className="text-gray-600">Ajoutez vos propres recettes et partagez-les avec la communauté.</p>
            </div>
          </div>
        </div>
      </section>
      <section className="bg-gray-50 px-6 py-16">
        <div className="container mx-auto text-center">
          <h2 className="mb-6 text-3xl font-bold text-gray-800">Générez des recettes avec l'IA</h2>
          <p className="mb-8 text-lg text-gray-700">
            Vous avez une liste d'ingrédients à disposition ? Notre IA est là pour vous aider à créer une recette unique basée sur ce que vous avez sous la main. Vous pouvez également la laisser vous
            inspirer en cuisine en générant une recette à partir d’un nom, d’un thème, ou même en améliorant une recette existante. Faites confiance à l'IA pour transformer vos idées en délicieux
            plats !
          </p>
          <Link to="/recipes/generator" className="rounded bg-indigo-600 px-6 py-3 text-lg font-medium text-white hover:bg-indigo-700">
            Générer une Recette
          </Link>
        </div>
      </section>
    </div>
  );
}

export default Home;
