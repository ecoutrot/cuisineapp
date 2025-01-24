import { RecipeCategory } from "../types/RecipeCategory";
import api from "./Axios";

const API_URL = "/recipeCategories";

export async function fetchRecipeCategoriesApi(page?: number | null, limit?: number | null): Promise<RecipeCategory[] | null> {
  try {
    const response = await api.get(API_URL, {
      params: { page, limit },
    });
    return response.data as RecipeCategory[];
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchRecipeCategoryApi(id: string): Promise<RecipeCategory | null> {
  try {
    const response = await api.get(`${API_URL}/${id}`);
    return response.data as RecipeCategory;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function createRecipeCategoryApi(recipeCategory: RecipeCategory): Promise<RecipeCategory | null> {
  try {
    const response = await api.post(API_URL, recipeCategory);
    return response.data as RecipeCategory;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function updateRecipeCategoryApi(recipeCategory: RecipeCategory): Promise<RecipeCategory | null> {
  try {
    const response = await api.put(`${API_URL}/${recipeCategory.id}`, recipeCategory);
    return response.data as RecipeCategory;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function deleteRecipeCategoryApi(id: string): Promise<void> {
  try {
    await api.delete(`${API_URL}/${id}`);
    return;
  } catch (error) {
    console.log(error);
    return;
  }
}
