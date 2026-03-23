import { createFileRoute, redirect } from "@tanstack/react-router";
import { isAuthenticated } from "@/lib/auth";
import { SignUpPage } from "@/pages/Auth/sign-up-page";

export const Route = createFileRoute("/sign-up")({
  beforeLoad: () => {
    if (isAuthenticated()) {
      throw redirect({ to: "/persons" });
    }
  },
  component: SignUpPage,
});
