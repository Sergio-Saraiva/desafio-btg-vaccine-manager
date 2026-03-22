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

export interface Vaccine {
    id: string;
    name: string;
    requiredDoses: number;
    code: string;
    createdAt: string;
}

export interface CreateVaccinePayload {
    name: string;
    requiredDoses: number;
    code?: string;
}

export interface UpdateVaccinePayload {
    id: string;
    name: string;
    requiredDoses: number;
}

export interface DoseRecord {
    recordId: string;
    applicationDate: string;
}

export interface VaccinationCardEntry {
    vaccineId: string;
    vaccineName: string;
    vaccineCode: string;
    requiredDoses: number;
    dosesTaken: number;
    isComplete: boolean;
    doses: DoseRecord[];
}

export interface VaccinationCardResponse {
    personId: string;
    personName: string;
    documentType: string;
    documentNumber: string;
    vaccines: VaccinationCardEntry[];
}

export interface CreateVaccinationRecordPayload {
    personId: string;
    vaccineId: string;
}