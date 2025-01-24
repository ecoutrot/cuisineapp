import { Ingredient } from "../types/Ingredient";
import api from "./Axios";

const API_URL = "/ingredients";

export async function fetchIngredientsApi(page?: number | null, limit?: number | null, search?: string | null): Promise<Ingredient[] | null> {
  try {
    const response = await api.get(API_URL, {
      params: { page, limit, search },
    });
    const ingredients = response.data as Ingredient[];
    const orderedIngredients = ingredients.sort((a: Ingredient, b: Ingredient) => a.name?.localeCompare(b.name ?? "") ?? 0);
    return orderedIngredients;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchIngredientApi(id: string): Promise<Ingredient | null> {
  try {
    const response = await api.get(`${API_URL}/${id}`);
    return response.data as Ingredient;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function createIngredientApi(ingredient: Ingredient): Promise<Ingredient | null> {
  try {
    const response = await api.post(API_URL, ingredient);
    return response.data as Ingredient;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function updateIngredientApi(ingredient: Ingredient): Promise<Ingredient | null> {
  try {
    const response = await api.put(`${API_URL}/${ingredient.id}`, ingredient);
    return response.data as Ingredient;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function deleteIngredientApi(id: string): Promise<void> {
  try {
    await api.delete(`${API_URL}/${id}`);
    return;
  } catch (error) {
    console.log(error);
    return;
  }
}

export async function importIngredientsApi(letter: string, i: number): Promise<Ingredient[] | null> {
  try {
    const response = await api.get(`${API_URL}/requete/${letter}/${i}`);
    return response.data as Ingredient[];
  } catch (error) {
    console.log(error);
    return null;
  }
}
