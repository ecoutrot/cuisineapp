import { Recipe } from "../types/Recipe";
import api from "./Axios";

const API_URL = "/recipes";

export async function fetchRecipesApi(page?: number | null, limit?: number | null, search?: string | null): Promise<Recipe[] | null> {
  try {
    const response = await api.get(API_URL, {
      params: { page, limit, search },
    });
    const recipes = response.data as Recipe[];
    const orderedRecipes = recipes.sort((a: Recipe, b: Recipe) => a.name?.localeCompare(b.name ?? "") ?? 0);
    return orderedRecipes;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchRecipeApi(id: string): Promise<Recipe | null> {
  try {
    const response = await api.get(`${API_URL}/${id}`);
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function createRecipeApi(recipe: Recipe): Promise<Recipe | null> {
  try {
    const response = await api.post(API_URL, recipe);
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function updateRecipeApi(recipe: Recipe): Promise<Recipe | null> {
  try {
    const response = await api.put(`${API_URL}/${recipe.id}`, recipe);
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function deleteRecipeApi(id: string): Promise<void> {
  try {
    await api.delete(`${API_URL}/${id}`);
    return;
  } catch (error) {
    console.log(error);
    return;
  }
}
