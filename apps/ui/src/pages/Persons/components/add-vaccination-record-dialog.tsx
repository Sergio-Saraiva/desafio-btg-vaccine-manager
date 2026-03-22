import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useVaccines } from "@/hooks/use-vaccines";

interface AddVaccinationRecordDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (vaccineId: string) => void;
  isLoading?: boolean;
}

export function AddVaccinationRecordDialog({
  open,
  onOpenChange,
  onSubmit,
  isLoading,
}: AddVaccinationRecordDialogProps) {
  const [vaccineId, setVaccineId] = useState("");
  const { data: response } = useVaccines({ pageSize: 100 });
  const vaccines = response?.data?.items ?? [];

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (vaccineId) {
      onSubmit(vaccineId);
      setVaccineId("");
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add Vaccination Record</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="vaccine">Vaccine</Label>
            <Select value={vaccineId} onValueChange={setVaccineId}>
              <SelectTrigger>
                <SelectValue placeholder="Select a vaccine" />
              </SelectTrigger>
              <SelectContent>
                {vaccines.map((v) => (
                  <SelectItem key={v.id} value={v.id}>
                    {v.name} ({v.code})
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={() => onOpenChange(false)}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={isLoading || !vaccineId}>
              {isLoading ? "Saving..." : "Add Record"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
