import { Unit } from "../types/Unit";
import api from "./Axios";

const API_URL = "/units";

export async function fetchUnitsApi(page?: number | null, limit?: number | null): Promise<Unit[] | null> {
  try {
    const response = await api.get(API_URL, {
      params: { page, limit },
    });
    return response.data as Unit[];
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchUnitApi(id: string): Promise<Unit | null> {
  try {
    const response = await api.get(`${API_URL}/${id}`);
    return response.data as Unit;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function createUnitApi(unit: Unit): Promise<Unit | null> {
  try {
    const response = await api.post(API_URL, unit);
    return response.data as Unit;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function updateUnitApi(unit: Unit): Promise<Unit | null> {
  try {
    const response = await api.put(`${API_URL}/${unit.id}`, unit);
    return response.data as Unit;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function deleteUnitApi(id: string): Promise<void> {
  try {
    await api.delete(`${API_URL}/${id}`);
    return;
  } catch (error) {
    console.log(error);
    return;
  }
}
