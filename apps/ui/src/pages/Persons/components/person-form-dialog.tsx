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
    { value: "0", label: "CPF" },
    { value: "1", label: "Passport" },
  ];

  export function PersonFormDialog({
    open,
    onOpenChange,
    onSubmit,
    person,
    isLoading,
  }: PersonFormDialogProps) {
    const [name, setName] = useState("");
    const [documentType, setDocumentType] = useState("0");
    const [documentNumber, setDocumentNumber] = useState("");
    const [nationality, setNationality] = useState("");

    useEffect(() => {
      if (person) {
        setName(person.name);
        setDocumentType(person.documentType === "Cpf" ? "0" : "1");
        setDocumentNumber(person.documentNumber);
        setNationality(person.nationality ?? "");
      } else {
        setName("");
        setDocumentType("0");
        setDocumentNumber("");
        setNationality("");
      }
    }, [person, open]);

    const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();
      onSubmit({
        name,
        documentType: Number(documentType),
        documentNumber,
        nationality,
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
                required
              />
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
                required
              />
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
