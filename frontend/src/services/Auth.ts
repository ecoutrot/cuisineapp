import { Token } from "../types/Token";
import api from "./Axios";

const API_URL = "/auth";

export async function fetchForRegister(credentials: { username: string; password: string }): Promise<Token | null> {
  try {
    const response = await api.post(`${API_URL}/register`, credentials);
    return response.data as Token;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchForAccessToken(credentials: { username: string; password: string }): Promise<Token | null> {
  try {
    const response = await api.post(`${API_URL}/login`, credentials);
    return response.data as Token;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchForRefreshToken(): Promise<Token | null> {
  try {
    const response = await api.post(`${API_URL}/refresh`);
    return response.data as Token;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchForLogout(): Promise<void> {
  try {
    await api.post(`${API_URL}/logout`, {});
    return;
  } catch (error) {
    console.log(error);
    return;
  }
}

export async function fetchForChangePassword(credentials: { oldPassword: string; newPassword: string }): Promise<Token | null> {
  try {
    const response = await api.post(`${API_URL}/passwordchange`, credentials);
    return response.data as Token;
  } catch (error) {
    console.log(error);
    return null;
  }
}
