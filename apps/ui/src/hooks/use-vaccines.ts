import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { vaccineService } from "../services/vaccine-service";
import type {
    CreateVaccinePayload,
    UpdateVaccinePayload,
    SieveParams,
} from "../types/api";

export function useVaccines(params?: SieveParams) {
    return useQuery({
        queryKey: ["vaccines", params],
        queryFn: () => vaccineService.list(params),
    });
}

export function useCreateVaccine() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (payload: CreateVaccinePayload) =>
            vaccineService.create(payload),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["vaccines"] });
        },
    });
}

export function useUpdateVaccine() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (payload: UpdateVaccinePayload) =>
            vaccineService.update(payload),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["vaccines"] });
        },
    });
}

export function useDeleteVaccine() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => vaccineService.delete(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["vaccines"] });
        },
    });
}
