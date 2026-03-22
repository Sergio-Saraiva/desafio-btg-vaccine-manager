import { useState } from "react";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { getApiErrorMessage } from "@/lib/utils";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import {
  useVaccines,
  useCreateVaccine,
  useUpdateVaccine,
  useDeleteVaccine,
} from "@/hooks/use-vaccines";
import { useDebouncedValue } from "@/hooks/use-debounced-value";
import type { Vaccine, SieveParams } from "@/types/api";
import { VaccineFormDialog } from "./components/vaccine-form-dialog";
import { DeleteVaccineDialog } from "./components/delete-vaccine-dialog";
import {
  ArrowUpDown,
  ArrowUp,
  ArrowDown,
  Search,
  X,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

type SortState = { field: string; direction: "asc" | "desc" } | null;

interface Filters {
  name: string;
  code: string;
}

const EMPTY_FILTERS: Filters = { name: "", code: "" };
const PAGE_SIZE = 10;

function buildSieveParams(
  filters: Filters,
  sort: SortState,
  page: number
): SieveParams {
  const parts: string[] = [];

  if (filters.name.trim()) parts.push(`Name@=*${filters.name.trim()}`);
  if (filters.code.trim()) parts.push(`Code@=*${filters.code.trim()}`);

  return {
    filters: parts.length > 0 ? parts.join(",") : undefined,
    sorts: sort
      ? sort.direction === "desc"
        ? `-${sort.field}`
        : sort.field
      : undefined,
    page,
    pageSize: PAGE_SIZE,
  };
}

function SortableHeader({
  label,
  field,
  sort,
  onSort,
}: {
  label: string;
  field: string;
  sort: SortState;
  onSort: (field: string) => void;
}) {
  const isActive = sort?.field === field;
  const Icon = isActive
    ? sort.direction === "asc"
      ? ArrowUp
      : ArrowDown
    : ArrowUpDown;

  return (
    <TableHead>
      <button
        className="flex items-center gap-1 hover:text-foreground transition-colors"
        onClick={() => onSort(field)}
      >
        {label}
        <Icon
          className={`size-4 ${isActive ? "text-foreground" : "text-muted-foreground"}`}
        />
      </button>
    </TableHead>
  );
}

function FilterInput({
  placeholder,
  value,
  onChange,
}: {
  placeholder: string;
  value: string;
  onChange: (value: string) => void;
}) {
  return (
    <div className="relative flex-1">
      <Search className="absolute left-2.5 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
      <Input
        placeholder={placeholder}
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="pl-8"
      />
      {value && (
        <button
          onClick={() => onChange("")}
          className="absolute right-2 top-1/2 -translate-y-1/2 text-muted-foreground hover:text-foreground"
        >
          <X className="size-4" />
        </button>
      )}
    </div>
  );
}

export function VaccinesPage() {
  const [formOpen, setFormOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [selectedVaccine, setSelectedVaccine] = useState<Vaccine | null>(null);
  const [filters, setFilters] = useState<Filters>(EMPTY_FILTERS);
  const [sort, setSort] = useState<SortState>(null);
  const [page, setPage] = useState(1);

  const debouncedFilters: Filters = {
    name: useDebouncedValue(filters.name, 300),
    code: useDebouncedValue(filters.code, 300),
  };
  const sieveParams = buildSieveParams(debouncedFilters, sort, page);

  const { data: response, isLoading } = useVaccines(sieveParams);
  const createVaccine = useCreateVaccine();
  const updateVaccine = useUpdateVaccine();
  const deleteVaccine = useDeleteVaccine();

  const paged = response?.data;
  const vaccines = paged?.items ?? [];
  const totalPages = paged?.totalPages ?? 1;
  const totalCount = paged?.totalCount ?? 0;

  const hasFilters = filters.name || filters.code || sort;

  const updateFilter = (key: keyof Filters, value: string) => {
    setFilters((prev) => ({ ...prev, [key]: value }));
    setPage(1);
  };

  const clearFilters = () => {
    setFilters(EMPTY_FILTERS);
    setSort(null);
    setPage(1);
  };

  const handleCreate = () => {
    setSelectedVaccine(null);
    setFormOpen(true);
  };

  const handleEdit = (vaccine: Vaccine) => {
    setSelectedVaccine(vaccine);
    setFormOpen(true);
  };

  const handleDelete = (vaccine: Vaccine) => {
    setSelectedVaccine(vaccine);
    setDeleteOpen(true);
  };

  const handleFormSubmit = (data: {
    name: string;
    requiredDoses: number;
    code?: string;
    id?: string;
  }) => {
    const mutation = data.id
      ? updateVaccine.mutateAsync({
          id: data.id,
          name: data.name,
          requiredDoses: data.requiredDoses,
        })
      : createVaccine.mutateAsync({
          name: data.name,
          requiredDoses: data.requiredDoses,
          code: data.code,
        });

    mutation
      .then(() => {
        setFormOpen(false);
        toast.success(data.id ? "Vaccine updated" : "Vaccine created");
      })
      .catch((error) => toast.error(getApiErrorMessage(error)));
  };

  const handleSort = (field: string) => {
    setSort((prev) => {
      if (prev?.field !== field) return { field, direction: "asc" };
      if (prev.direction === "asc") return { field, direction: "desc" };
      return null;
    });
  };

  const handleDeleteConfirm = () => {
    if (selectedVaccine) {
      deleteVaccine
        .mutateAsync(selectedVaccine.id)
        .then(() => {
          setDeleteOpen(false);
          toast.success("Vaccine deleted");
        })
        .catch((error) => toast.error(getApiErrorMessage(error)));
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Vaccines</h1>
        <Button onClick={handleCreate}>New Vaccine</Button>
      </div>

      <div className="flex items-center gap-2">
        <FilterInput
          placeholder="Search by name..."
          value={filters.name}
          onChange={(v) => updateFilter("name", v)}
        />
        <FilterInput
          placeholder="Search by code..."
          value={filters.code}
          onChange={(v) => updateFilter("code", v)}
        />
        {hasFilters && (
          <Button variant="ghost" size="sm" onClick={clearFilters}>
            Clear filters
          </Button>
        )}
      </div>

      {isLoading ? (
        <p className="text-muted-foreground">Loading...</p>
      ) : vaccines.length === 0 ? (
        <p className="text-muted-foreground">No vaccines found.</p>
      ) : (
        <>
          <Table>
            <TableHeader>
              <TableRow>
                <SortableHeader
                  label="Name"
                  field="Name"
                  sort={sort}
                  onSort={handleSort}
                />
                <TableHead>Code</TableHead>
                <SortableHeader
                  label="Required Doses"
                  field="RequiredDoses"
                  sort={sort}
                  onSort={handleSort}
                />
                <TableHead className="w-[150px]">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {vaccines.map((vaccine) => (
                <TableRow key={vaccine.id}>
                  <TableCell className="font-medium">{vaccine.name}</TableCell>
                  <TableCell>
                    <Badge variant="secondary">{vaccine.code}</Badge>
                  </TableCell>
                  <TableCell>{vaccine.requiredDoses}</TableCell>
                  <TableCell>
                    <div className="flex gap-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleEdit(vaccine)}
                      >
                        Edit
                      </Button>
                      <Button
                        variant="destructive"
                        size="sm"
                        onClick={() => handleDelete(vaccine)}
                      >
                        Delete
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>

          <div className="flex items-center justify-between">
            <p className="text-sm text-muted-foreground">
              {totalCount} result{totalCount !== 1 && "s"}
            </p>
            <div className="flex items-center gap-2">
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage((p) => p - 1)}
                disabled={page <= 1}
              >
                <ChevronLeft className="size-4" />
                Previous
              </Button>
              <span className="text-sm text-muted-foreground">
                Page {page} of {totalPages}
              </span>
              <Button
                variant="outline"
                size="sm"
                onClick={() => setPage((p) => p + 1)}
                disabled={page >= totalPages}
              >
                Next
                <ChevronRight className="size-4" />
              </Button>
            </div>
          </div>
        </>
      )}

      <VaccineFormDialog
        open={formOpen}
        onOpenChange={setFormOpen}
        onSubmit={handleFormSubmit}
        vaccine={selectedVaccine}
        isLoading={createVaccine.isPending || updateVaccine.isPending}
      />

      <DeleteVaccineDialog
        open={deleteOpen}
        onOpenChange={setDeleteOpen}
        onConfirm={handleDeleteConfirm}
        vaccine={selectedVaccine}
        isLoading={deleteVaccine.isPending}
      />
    </div>
  );
}
