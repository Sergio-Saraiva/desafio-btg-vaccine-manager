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
import type { Vaccine } from "@/types/api";

interface VaccineFormDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: {
    name: string;
    requiredDoses: number;
    code?: string;
    id?: string;
  }) => void;
  vaccine?: Vaccine | null;
  isLoading?: boolean;
}

type FormErrors = Partial<Record<"name" | "requiredDoses" | "code", string>>;

function validate(
  name: string,
  requiredDoses: string,
  code: string,
  isEdit: boolean
): FormErrors {
  const errors: FormErrors = {};

  if (!name.trim()) {
    errors.name = "Name is required";
  }

  const doses = Number(requiredDoses);
  if (!requiredDoses || isNaN(doses) || doses < 1) {
    errors.requiredDoses = "Required doses must be at least 1";
  }

  if (!isEdit && code && code.length > 20) {
    errors.code = "Code must be at most 20 characters";
  }

  return errors;
}

export function VaccineFormDialog({
  open,
  onOpenChange,
  onSubmit,
  vaccine,
  isLoading,
}: VaccineFormDialogProps) {
  const [name, setName] = useState("");
  const [requiredDoses, setRequiredDoses] = useState("1");
  const [code, setCode] = useState("");
  const [errors, setErrors] = useState<FormErrors>({});

  useEffect(() => {
    if (vaccine) {
      setName(vaccine.name);
      setRequiredDoses(String(vaccine.requiredDoses));
      setCode(vaccine.code);
    } else {
      setName("");
      setRequiredDoses("1");
      setCode("");
    }
    setErrors({});
  }, [vaccine, open]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const validationErrors = validate(name, requiredDoses, code, !!vaccine);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    setErrors({});
    onSubmit({
      name: name.trim(),
      requiredDoses: Number(requiredDoses),
      ...(!vaccine && code ? { code: code.trim() } : {}),
      ...(vaccine && { id: vaccine.id }),
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>
            {vaccine ? "Edit Vaccine" : "New Vaccine"}
          </DialogTitle>
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
            <Label htmlFor="requiredDoses">Required Doses</Label>
            <Input
              id="requiredDoses"
              type="number"
              min="1"
              value={requiredDoses}
              onChange={(e) => setRequiredDoses(e.target.value)}
              aria-invalid={!!errors.requiredDoses}
            />
            {errors.requiredDoses && (
              <p className="text-sm text-destructive">
                {errors.requiredDoses}
              </p>
            )}
          </div>
          {!vaccine && (
            <div className="space-y-2">
              <Label htmlFor="code">Code (optional)</Label>
              <Input
                id="code"
                value={code}
                onChange={(e) => setCode(e.target.value)}
                placeholder="Auto-generated if empty"
                aria-invalid={!!errors.code}
              />
              {errors.code && (
                <p className="text-sm text-destructive">{errors.code}</p>
              )}
            </div>
          )}
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
