export interface ApiResponse<T> {
    isSuccess: boolean;
    data: T | null;
    errorMessage: string | null;
  }

export interface Person {
    id: string;
    name: string;
    documentType: string;
    documentNumber: string;
    nationality: string | null;
}

export interface CreatePersonPayload {
    name: string;
    documentType: number;
    documentNumber: string;
    nationality: string;
}

export interface UpdatePersonPayload extends CreatePersonPayload {
    id: string;
}

export interface SieveParams {
    filters?: string;
    sorts?: string;
    page?: number;
    pageSize?: number;
}

export interface PagedResponse<T> {
    items: T[];
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}