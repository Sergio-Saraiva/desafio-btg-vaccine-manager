import { createFileRoute, Navigate } from "@tanstack/react-router";
import { isAuthenticated } from "@/lib/auth";

export const Route = createFileRoute("/")({
  component: () =>
    isAuthenticated() ? (
      <Navigate to="/persons" />
    ) : (
      <Navigate to="/sign-in" />
    ),
});
