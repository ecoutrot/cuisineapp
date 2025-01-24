import { createContext, useContext, useLayoutEffect, useMemo, useState } from "react";
import { fetchForRefreshToken } from "../../services/Auth";
import api from "../../services/Axios";

import { ReactNode } from "react";
import { useNavigate } from "react-router-dom";
import { AxiosError, InternalAxiosRequestConfig } from "axios";

interface AuthContextType {
  token: string | null | undefined;
  setToken: React.Dispatch<React.SetStateAction<string | null>>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const authContext = useContext(AuthContext);
  if (!authContext) {
    throw new Error("useContext is undefined");
  }
  return authContext;
};

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem("token"));
  const navigate = useNavigate();

  useLayoutEffect(() => {
    const authInterceptor = api.interceptors.request.use((config) => {
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });
    return () => {
      api.interceptors.request.eject(authInterceptor);
    };
  }, [token]);

  useLayoutEffect(() => {
    const refreshInterceptor = api.interceptors.response.use(
      (response) => {
        return response;
      },
      async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };
    
        if (error.response && error.response.status === 401 && originalRequest && !originalRequest._retry) {
          originalRequest._retry = true;
    
          try {
            const newToken = await fetchForRefreshToken();
            if (!newToken) {
              logout();
              void navigate("/login");
              return;
            }
            setToken(newToken);
            localStorage.setItem("token", newToken);
            return api(originalRequest);
          } catch (err) {
            if ((err as AxiosError<{ message: string }>).response?.data?.message === "Invalid refresh token.") {
              void navigate("/login");
              return;
            }
            return Promise.reject(err as Error);
          }
        }
        return Promise.reject(error as Error);
      }
    );

    return () => {
      api.interceptors.response.eject(refreshInterceptor);
    };
  }, [token, navigate]);

  const logout = () => {
    setToken(null);
    localStorage.removeItem("token");
  };

  const contextValue = useMemo(
    () => ({
      token,
      setToken,
      logout,
    }),
    [token]
  );

  return <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>;
};

export default AuthProvider;
