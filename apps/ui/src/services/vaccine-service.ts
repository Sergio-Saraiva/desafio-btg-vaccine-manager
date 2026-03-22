import { api } from "../lib/axios";
import type {
    ApiResponse,
    PagedResponse,
    Vaccine,
    CreateVaccinePayload,
    UpdateVaccinePayload,
    SieveParams,
} from "../types/api";

export const vaccineService = {
    list: async (params?: SieveParams) => {
        const { data } = await api.get<ApiResponse<PagedResponse<Vaccine>>>("/Vaccines", {
            params,
        });
        return data;
    },

    create: async (payload: CreateVaccinePayload) => {
        const { data } = await api.post<ApiResponse<Vaccine>>("/Vaccines", payload);
        return data;
    },

    update: async (payload: UpdateVaccinePayload) => {
        const { data } = await api.put<ApiResponse<Vaccine>>("/Vaccines", payload);
        return data;
    },

    delete: async (id: string) => {
        const { data } = await api.delete<ApiResponse<null>>(`/Vaccines/${id}`);
        return data;
    },
};
