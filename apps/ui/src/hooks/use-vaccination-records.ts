import { useMutation, useQueryClient } from "@tanstack/react-query";
import { vaccinationRecordService } from "../services/vaccination-record-service";
import type { CreateVaccinationRecordPayload } from "../types/api";

export function useCreateVaccinationRecord() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (payload: CreateVaccinationRecordPayload) =>
            vaccinationRecordService.create(payload),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["vaccination-card"] });
        },
    });
}

export function useDeleteVaccinationRecord() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => vaccinationRecordService.delete(id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["vaccination-card"] });
        },
    });
}
