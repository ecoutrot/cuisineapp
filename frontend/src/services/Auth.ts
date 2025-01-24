import api from "./Axios";

const API_URL = "/auth";

export async function fetchForRegister(credentials: { username: string; password: string }): Promise<string | null> {
  try {
    const response = await api.post(`${API_URL}/register`, credentials);
    return response.data as string;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchForAccessToken(credentials: { username: string; password: string }): Promise<string | null> {
  try {
    const response = await api.post(`${API_URL}/login`, credentials);
    return response.data as string;
  } catch (error) {
    console.log(error);
    return null;
  }
}

export async function fetchForRefreshToken(): Promise<string | null> {
  try {
    const response = await api.post(`${API_URL}/refresh`);
    return response.data as string;
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

export async function fetchForChangePassword(credentials: { oldPassword: string; newPassword: string }): Promise<string | null> {
  try {
    const response = await api.post(`${API_URL}/passwordchange`, credentials);
    return response.data as string;
  } catch (error) {
    console.log(error);
    return null;
  }
}
