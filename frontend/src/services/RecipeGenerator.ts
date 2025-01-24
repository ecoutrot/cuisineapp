import api from "./Axios";
import { Recipe } from "../types/Recipe";

const API_URL = "/recipeGenerator";

export async function generateRecipeIdeaApi(idea: string): Promise<Recipe | null> {
  try {
    const response = await api.post(`${API_URL}/idea`, idea, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function generateRecipeIngredientsApi(listIngredients: string[]): Promise<Recipe | null> {
  try {
    const response = await api.post(`${API_URL}/list`, listIngredients);
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function generateRecipeImproveApi(id: string): Promise<Recipe | null> {
  try {
    const response = await api.get(`${API_URL}/improve/${id}`);
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function generateRecipeImageApi(title: string, image: File): Promise<Recipe | null> {
  try {
    const formData = new FormData();
    formData.append("title", title);
    formData.append("image", image);
    const response = await api.post(`${API_URL}/image`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
    return response.data as Recipe;
  } catch (error) {
    console.log(error);
    return null;
  }
}
