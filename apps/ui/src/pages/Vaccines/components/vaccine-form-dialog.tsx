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
  }, [vaccine, open]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      name,
      requiredDoses: Number(requiredDoses),
      ...(!vaccine && code ? { code } : {}),
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
              required
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="requiredDoses">Required Doses</Label>
            <Input
              id="requiredDoses"
              type="number"
              min="1"
              value={requiredDoses}
              onChange={(e) => setRequiredDoses(e.target.value)}
              required
            />
          </div>
          {!vaccine && (
            <div className="space-y-2">
              <Label htmlFor="code">Code (optional)</Label>
              <Input
                id="code"
                value={code}
                onChange={(e) => setCode(e.target.value)}
                placeholder="Auto-generated if empty"
              />
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
