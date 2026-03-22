import { clsx, type ClassValue } from "clsx"
import { AxiosError } from "axios"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function getApiErrorMessage(error: unknown): string {
  if (error instanceof AxiosError) {
    const data = error.response?.data;
    if (data?.errorMessage) return data.errorMessage;
  }
  if (error instanceof Error) return error.message;
  return "An unexpected error occurred";
}
