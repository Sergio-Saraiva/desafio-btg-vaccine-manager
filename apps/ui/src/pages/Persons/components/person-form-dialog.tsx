import { useEffect, useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import type { Person } from "@/types/api";

interface PersonFormDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: {
    name: string;
    documentType: number;
    documentNumber: string;
    nationality: string;
    id?: string;
  }) => void;
  person?: Person | null;
  isLoading?: boolean;
}

const DOCUMENT_TYPES = [
  { value: "1", label: "CPF" },
  { value: "2", label: "Passport" },
];

type FormErrors = Partial<Record<"name" | "documentNumber", string>>;

function validate(
  name: string,
  documentType: string,
  documentNumber: string
): FormErrors {
  const errors: FormErrors = {};

  if (!name.trim()) {
    errors.name = "Name is required";
  } else if (name.trim().length < 2) {
    errors.name = "Name must be at least 2 characters";
  }

  if (!documentNumber.trim()) {
    errors.documentNumber = "Document number is required";
  } else if (documentType === "1") {
    const digits = documentNumber.replace(/\D/g, "");
    if (digits.length !== 11) {
      errors.documentNumber = "CPF must have exactly 11 digits";
    }
  } else if (documentType === "2") {
    const cleaned = documentNumber.trim();
    if (cleaned.length < 6 || cleaned.length > 15) {
      errors.documentNumber =
        "Passport number must be between 6 and 15 characters";
    }
  }

  return errors;
}

export function PersonFormDialog({
  open,
  onOpenChange,
  onSubmit,
  person,
  isLoading,
}: PersonFormDialogProps) {
  const [name, setName] = useState("");
  const [documentType, setDocumentType] = useState("1");
  const [documentNumber, setDocumentNumber] = useState("");
  const [nationality, setNationality] = useState("");
  const [errors, setErrors] = useState<FormErrors>({});

  useEffect(() => {
    if (person) {
      setName(person.name);
      setDocumentType(person.documentType === "Cpf" ? "1" : "2");
      setDocumentNumber(person.documentNumber);
      setNationality(person.nationality ?? "");
    } else {
      setName("");
      setDocumentType("1");
      setDocumentNumber("");
      setNationality("");
    }
    setErrors({});
  }, [person, open]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const validationErrors = validate(name, documentType, documentNumber);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    setErrors({});
    onSubmit({
      name: name.trim(),
      documentType: Number(documentType),
      documentNumber: documentNumber.trim(),
      nationality: nationality.trim(),
      ...(person && { id: person.id }),
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{person ? "Edit Person" : "New Person"}</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Name</Label>
            <Input
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              aria-invalid={!!errors.name}
            />
            {errors.name && (
              <p className="text-sm text-destructive">{errors.name}</p>
            )}
          </div>
          <div className="space-y-2">
            <Label htmlFor="documentType">Document Type</Label>
            <Select value={documentType} onValueChange={setDocumentType}>
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                {DOCUMENT_TYPES.map((dt) => (
                  <SelectItem key={dt.value} value={dt.value}>
                    {dt.label}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
          <div className="space-y-2">
            <Label htmlFor="documentNumber">Document Number</Label>
            <Input
              id="documentNumber"
              value={documentNumber}
              onChange={(e) => setDocumentNumber(e.target.value)}
              aria-invalid={!!errors.documentNumber}
            />
            {errors.documentNumber && (
              <p className="text-sm text-destructive">
                {errors.documentNumber}
              </p>
            )}
          </div>
          <div className="space-y-2">
            <Label htmlFor="nationality">Nationality</Label>
            <Input
              id="nationality"
              value={nationality}
              onChange={(e) => setNationality(e.target.value)}
            />
          </div>
          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={() => onOpenChange(false)}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={isLoading}>
              {isLoading ? "Saving..." : "Save"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
