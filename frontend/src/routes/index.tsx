import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import Login from "../pages/Home/Login";
import RecipeView from "../pages/Recipe/RecipeView";
import RecipesView from "../pages/Recipe/RecipesView";
import RecipeEdit from "../pages/Recipe/RecipeEdit";
import IngredientImport from "../pages/Ingredient/IngredientImport";
import RecipeCreate from "../pages/Recipe/RecipeCreate";
import IngredientCreate from "../pages/Ingredient/IngredientCreate";
import IngredientEdit from "../pages/Ingredient/IngredientEdit";
import IngredientsView from "../pages/Ingredient/IngredientsView";
import UnitCreate from "../pages/Unit/UnitCreate";
import UnitEdit from "../pages/Unit/UnitEdit";
import UnitsView from "../pages/Unit/UnitsView";
import RecipeCategoriesView from "../pages/RecipeCategory/RecipeCategoriesView";
import RecipeCategoryCreate from "../pages/RecipeCategory/RecipeCategoryCreate";
import RecipeCategoryEdit from "../pages/RecipeCategory/RecipeCategoryEdit";
import Home from "../pages/Home/Home";
import Register from "../pages/Home/Register";
import RecipeGenerator from "../pages/Recipe/RecipeGenerator";
import RecipeImproveGenerator from "../pages/Recipe/RecipeImproveGenerator";
import PasswordChange from "../pages/Home/PasswordChange";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
      {
        path: "login",
        element: <Login />,
      },
      {
        path: "password-change",
        element: <PasswordChange />,
      },
      {
        path: "register",
        element: <Register />,
      },
      {
        path: "recipes",
        element: <RecipesView />,
      },
      {
        path: "recipes/:id",
        element: <RecipeView />,
      },
      {
        path: "recipes/create",
        element: <RecipeCreate />,
      },
      {
        path: "recipes/edit/:id",
        element: <RecipeEdit />,
      },
      {
        path: "recipes/generator",
        element: <RecipeGenerator />,
      },
      {
        path: "recipes/generator/:id",
        element: <RecipeImproveGenerator />,
      },
      {
        path: "ingredients/import",
        element: <IngredientImport />,
      },
      {
        path: "ingredients",
        element: <IngredientsView />,
      },
      {
        path: "ingredients/create",
        element: <IngredientCreate />,
      },
      {
        path: "ingredients/edit/:id",
        element: <IngredientEdit />,
      },
      {
        path: "units",
        element: <UnitsView />,
      },
      {
        path: "units/create",
        element: <UnitCreate />,
      },
      {
        path: "units/edit/:id",
        element: <UnitEdit />,
      },
      {
        path: "recipeCategories",
        element: <RecipeCategoriesView />,
      },
      {
        path: "recipeCategories/create",
        element: <RecipeCategoryCreate />,
      },
      {
        path: "recipeCategories/edit/:id",
        element: <RecipeCategoryEdit />,
      },
    ],
  },
]);
