import { createFileRoute, Outlet, redirect } from "@tanstack/react-router";
import { isAuthenticated } from "@/lib/auth";

export const Route = createFileRoute("/persons")({
  beforeLoad: () => {
    if (!isAuthenticated()) {
      throw redirect({ to: "/sign-in" });
    }
  },
  component: Outlet,
});
