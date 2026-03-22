import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
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
  usePersons,
  useCreatePerson,
  useUpdatePerson,
  useDeletePerson,
} from "@/hooks/use-persons";
import { useDebouncedValue } from "@/hooks/use-debounced-value";
import type { Person, SieveParams } from "@/types/api";
import { PersonFormDialog } from "./components/person-form-dialog";
import { DeletePersonDialog } from "./components/delete-person-dialog";
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
  document: string;
  nationality: string;
}

const EMPTY_FILTERS: Filters = { name: "", document: "", nationality: "" };
const PAGE_SIZE = 10;

function buildSieveParams(
  filters: Filters,
  sort: SortState,
  page: number
): SieveParams {
  const parts: string[] = [];

  if (filters.name.trim())
    parts.push(`Name@=*${filters.name.trim()}`);
  if (filters.document.trim())
    parts.push(`DocumentNumber@=*${filters.document.trim()}`);
  if (filters.nationality.trim())
    parts.push(`Nationality@=*${filters.nationality.trim()}`);

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

export function PersonsPage() {
  const [formOpen, setFormOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [selectedPerson, setSelectedPerson] = useState<Person | null>(null);
  const [filters, setFilters] = useState<Filters>(EMPTY_FILTERS);
  const [sort, setSort] = useState<SortState>(null);
  const [page, setPage] = useState(1);

  const debouncedFilters: Filters = {
    name: useDebouncedValue(filters.name, 300),
    document: useDebouncedValue(filters.document, 300),
    nationality: useDebouncedValue(filters.nationality, 300),
  };
  const sieveParams = buildSieveParams(debouncedFilters, sort, page);

  const { data: response, isLoading } = usePersons(sieveParams);
  const createPerson = useCreatePerson();
  const updatePerson = useUpdatePerson();
  const deletePerson = useDeletePerson();

  const paged = response?.data;
  const persons = paged?.items ?? [];
  const totalPages = paged?.totalPages ?? 1;
  const totalCount = paged?.totalCount ?? 0;

  const hasFilters =
    filters.name || filters.document || filters.nationality || sort;

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
    setSelectedPerson(null);
    setFormOpen(true);
  };

  const handleEdit = (person: Person) => {
    setSelectedPerson(person);
    setFormOpen(true);
  };

  const handleDelete = (person: Person) => {
    setSelectedPerson(person);
    setDeleteOpen(true);
  };

  const handleFormSubmit = (data: {
    name: string;
    documentType: number;
    documentNumber: string;
    nationality: string;
    id?: string;
  }) => {
    const mutation = data.id
      ? updatePerson.mutateAsync({
          id: data.id,
          name: data.name,
          documentType: data.documentType,
          documentNumber: data.documentNumber,
          nationality: data.nationality,
        })
      : createPerson.mutateAsync({
          name: data.name,
          documentType: data.documentType,
          documentNumber: data.documentNumber,
          nationality: data.nationality,
        });

    mutation.then(() => setFormOpen(false));
  };

  const handleSort = (field: string) => {
    setSort((prev) => {
      if (prev?.field !== field) return { field, direction: "asc" };
      if (prev.direction === "asc") return { field, direction: "desc" };
      return null;
    });
  };

  const handleDeleteConfirm = () => {
    if (selectedPerson) {
      deletePerson
        .mutateAsync(selectedPerson.id)
        .then(() => setDeleteOpen(false));
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold">Persons</h1>
        <Button onClick={handleCreate}>New Person</Button>
      </div>

      <div className="flex items-center gap-2">
        <FilterInput
          placeholder="Search by name..."
          value={filters.name}
          onChange={(v) => updateFilter("name", v)}
        />
        <FilterInput
          placeholder="Search by document..."
          value={filters.document}
          onChange={(v) => updateFilter("document", v)}
        />
        <FilterInput
          placeholder="Search by nationality..."
          value={filters.nationality}
          onChange={(v) => updateFilter("nationality", v)}
        />
        {hasFilters && (
          <Button variant="ghost" size="sm" onClick={clearFilters}>
            Clear filters
          </Button>
        )}
      </div>

      {isLoading ? (
        <p className="text-muted-foreground">Loading...</p>
      ) : persons.length === 0 ? (
        <p className="text-muted-foreground">No persons found.</p>
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
                <TableHead>Document</TableHead>
                <SortableHeader
                  label="Nationality"
                  field="Nationality"
                  sort={sort}
                  onSort={handleSort}
                />
                <TableHead className="w-[150px]">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {persons.map((person) => (
                <TableRow key={person.id}>
                  <TableCell className="font-medium">{person.name}</TableCell>
                  <TableCell>
                    <Badge variant="secondary">{person.documentType}</Badge>{" "}
                    {person.documentNumber}
                  </TableCell>
                  <TableCell>{person.nationality ?? "—"}</TableCell>
                  <TableCell>
                    <div className="flex gap-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleEdit(person)}
                      >
                        Edit
                      </Button>
                      <Button
                        variant="destructive"
                        size="sm"
                        onClick={() => handleDelete(person)}
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

      <PersonFormDialog
        open={formOpen}
        onOpenChange={setFormOpen}
        onSubmit={handleFormSubmit}
        person={selectedPerson}
        isLoading={createPerson.isPending || updatePerson.isPending}
      />

      <DeletePersonDialog
        open={deleteOpen}
        onOpenChange={setDeleteOpen}
        onConfirm={handleDeleteConfirm}
        person={selectedPerson}
        isLoading={deletePerson.isPending}
      />
    </div>
  );
}
