import api from "./Axios";
import { User } from "../types/User";

const API_URL = "/users";

export async function fetchUserApi(id: string): Promise<User | null> {
  try {
    const response = await api.get(`${API_URL}/${id}`);
    return response.data as User;
  } catch (error) {
    console.log(error);
    return null;
  }
}
