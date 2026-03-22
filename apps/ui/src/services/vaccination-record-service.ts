import { api } from "../lib/axios";
import type {
    ApiResponse,
    CreateVaccinationRecordPayload,
} from "../types/api";

export const vaccinationRecordService = {
    create: async (payload: CreateVaccinationRecordPayload) => {
        const { data } = await api.post<ApiResponse<unknown>>(
            "/VaccinationRecord",
            payload
        );
        return data;
    },

    delete: async (id: string) => {
        const { data } = await api.delete<ApiResponse<null>>(
            `/VaccinationRecord/${id}`
        );
        return data;
    },
};
