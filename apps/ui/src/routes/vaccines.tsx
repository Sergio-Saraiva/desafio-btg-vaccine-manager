import { VaccinesPage } from "@/pages/Vaccines/vaccines-page";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/vaccines")({
  component: VaccinesPage,
});
