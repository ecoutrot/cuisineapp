import axios from "axios";

const DEV_API_URL = "http://localhost:5266";
const PROD_API_URL = "YOUR_API_URL";

const API_URL = import.meta.env.MODE === "development" ? DEV_API_URL : PROD_API_URL;

const api = axios.create({
  baseURL: `${API_URL}/api`,
  withCredentials: true,
});

export default api;
