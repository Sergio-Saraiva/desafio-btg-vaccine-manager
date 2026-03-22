import { api } from "../lib/axios";
import type {
    ApiResponse,
    PagedResponse,
    Person,
    CreatePersonPayload,
    UpdatePersonPayload,
    SieveParams,
    VaccinationCardResponse,
} from "../types/api";

export const personService = {
    list: async (params?: SieveParams) => {
      const { data } = await api.get<ApiResponse<PagedResponse<Person>>>("/Persons", {
        params,
      });
      return data;
    },

    create: async (payload: CreatePersonPayload) => {
      const { data } = await api.post<ApiResponse<Person>>("/Persons", payload);
      return data;
    },

    update: async (payload: UpdatePersonPayload) => {
      const { data } = await api.put<ApiResponse<Person>>("/Persons", payload);
      return data;
    },

    delete: async (id: string) => {
      const { data } = await api.delete<ApiResponse<null>>(`/Persons/${id}`);
      return data;
    },

    getVaccinationCard: async (id: string) => {
      const { data } = await api.get<ApiResponse<VaccinationCardResponse>>(
        `/Persons/${id}/vaccination-card`
      );
      return data;
    },
};
