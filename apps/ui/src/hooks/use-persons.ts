import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { personService } from "../services/person-service";
import type {
    CreatePersonPayload,
    UpdatePersonPayload,
    SieveParams,
} from "../types/api";

export function usePersons(params?: SieveParams) {
    return useQuery({
        queryKey: ["persons", params],
        queryFn: () => personService.list(params),
    });
}

export function useVaccinationCard(personId: string) {
    return useQuery({
        queryKey: ["vaccination-card", personId],
        queryFn: () => personService.getVaccinationCard(personId),
    });
}

export function useCreatePerson() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (payload: CreatePersonPayload) =>
        personService.create(payload),
        onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ["persons"] });
        },
    });
}

export function useUpdatePerson() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (payload: UpdatePersonPayload) =>
        personService.update(payload),
        onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ["persons"] });
        },
    });
}

export function useDeletePerson() {
    const queryClient = useQueryClient();
    return useMutation({
        mutationFn: (id: string) => personService.delete(id),
        onSuccess: () => {
        queryClient.invalidateQueries({ queryKey: ["persons"] });
        },
    });
}