import { createFileRoute, redirect } from "@tanstack/react-router";
import { isAuthenticated } from "@/lib/auth";
import { VaccinesPage } from "@/pages/Vaccines/vaccines-page";

export const Route = createFileRoute("/vaccines")({
  beforeLoad: () => {
    if (!isAuthenticated()) {
      throw redirect({ to: "/sign-in" });
    }
  },
  component: VaccinesPage,
});
