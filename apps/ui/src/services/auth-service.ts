import { api } from "../lib/axios";
import type { ApiResponse, SignInResponse, CreateUserResponse } from "../types/api";

export const authService = {
  signUp: async (payload: { email: string; password: string; passwordConfirmation: string }) => {
    const { data } = await api.post<ApiResponse<CreateUserResponse>>("/Auth/signup", payload);
    return data;
  },

  signIn: async (payload: { email: string; password: string }) => {
    const { data } = await api.post<ApiResponse<SignInResponse>>("/Auth/signin", payload);
    return data;
  },
};
