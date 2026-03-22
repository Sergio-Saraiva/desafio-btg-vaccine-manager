import { useState } from "react";
import { Link } from "@tanstack/react-router";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { useVaccinationCard } from "@/hooks/use-persons";
import {
  useCreateVaccinationRecord,
  useDeleteVaccinationRecord,
} from "@/hooks/use-vaccination-records";
import { AddVaccinationRecordDialog } from "./components/add-vaccination-record-dialog";
import { ArrowLeft, Plus, Trash2, CheckCircle2, Circle } from "lucide-react";

export function PersonDetailsPage({ personId }: { personId: string }) {
  const [addOpen, setAddOpen] = useState(false);
  const { data: response, isLoading } = useVaccinationCard(personId);
  const createRecord = useCreateVaccinationRecord();
  const deleteRecord = useDeleteVaccinationRecord();

  const card = response?.data;

  const handleAddRecord = (vaccineId: string) => {
    createRecord
      .mutateAsync({ personId, vaccineId })
      .then(() => setAddOpen(false));
  };

  const handleDeleteRecord = (recordId: string) => {
    deleteRecord.mutateAsync(recordId);
  };

  if (isLoading) {
    return <p className="text-muted-foreground">Loading...</p>;
  }

  if (!card) {
    return <p className="text-muted-foreground">Person not found.</p>;
  }

  return (
    <div className="space-y-8">
      <div>
        <Link
          to="/persons"
          className="inline-flex items-center gap-1 text-sm text-muted-foreground hover:text-foreground mb-4"
        >
          <ArrowLeft className="size-4" />
          Back to Persons
        </Link>
        <h1 className="text-3xl font-bold">{card.personName}</h1>
      </div>

      <div className="grid grid-cols-3 gap-4 rounded-lg border p-4">
        <div>
          <p className="text-sm text-muted-foreground">Document</p>
          <p className="font-medium">
            <Badge variant="secondary">{card.documentType}</Badge>{" "}
            {card.documentNumber}
          </p>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">Vaccines Taken</p>
          <p className="font-medium">{card.vaccines.length}</p>
        </div>
        <div>
          <p className="text-sm text-muted-foreground">All Complete</p>
          <p className="font-medium">
            {card.vaccines.length > 0 &&
            card.vaccines.every((v) => v.isComplete)
              ? "Yes"
              : "No"}
          </p>
        </div>
      </div>

      <div className="space-y-4">
        <div className="flex items-center justify-between">
          <h2 className="text-xl font-semibold">Vaccination Card</h2>
          <Button onClick={() => setAddOpen(true)}>
            <Plus className="size-4" />
            Add Record
          </Button>
        </div>

        {card.vaccines.length === 0 ? (
          <p className="text-muted-foreground">
            No vaccination records yet.
          </p>
        ) : (
          <div className="space-y-4">
            {card.vaccines.map((entry) => (
              <div key={entry.vaccineId} className="rounded-lg border">
                <div className="flex items-center justify-between border-b px-4 py-3">
                  <div className="flex items-center gap-3">
                    {entry.isComplete ? (
                      <CheckCircle2 className="size-5 text-green-600" />
                    ) : (
                      <Circle className="size-5 text-muted-foreground" />
                    )}
                    <div>
                      <p className="font-medium">{entry.vaccineName}</p>
                      <p className="text-sm text-muted-foreground">
                        {entry.vaccineCode}
                      </p>
                    </div>
                  </div>
                  <div className="text-right">
                    <Badge
                      variant={entry.isComplete ? "default" : "secondary"}
                    >
                      {entry.dosesTaken} / {entry.requiredDoses} doses
                    </Badge>
                  </div>
                </div>
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Dose</TableHead>
                      <TableHead>Application Date</TableHead>
                      <TableHead className="w-[80px]">Actions</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {entry.doses.map((dose, index) => (
                      <TableRow key={dose.recordId}>
                        <TableCell>Dose {index + 1}</TableCell>
                        <TableCell>
                          {new Date(dose.applicationDate).toLocaleDateString()}
                        </TableCell>
                        <TableCell>
                          <Button
                            variant="ghost"
                            size="icon-sm"
                            onClick={() => handleDeleteRecord(dose.recordId)}
                            disabled={deleteRecord.isPending}
                          >
                            <Trash2 className="size-4 text-destructive" />
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            ))}
          </div>
        )}
      </div>

      <AddVaccinationRecordDialog
        open={addOpen}
        onOpenChange={setAddOpen}
        onSubmit={handleAddRecord}
        isLoading={createRecord.isPending}
      />
    </div>
  );
}
