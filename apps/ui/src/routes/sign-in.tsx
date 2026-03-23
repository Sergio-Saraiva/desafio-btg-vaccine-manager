import { createFileRoute, redirect } from "@tanstack/react-router";
import { isAuthenticated } from "@/lib/auth";
import { SignInPage } from "@/pages/Auth/sign-in-page";

export const Route = createFileRoute("/sign-in")({
  beforeLoad: () => {
    if (isAuthenticated()) {
      throw redirect({ to: "/persons" });
    }
  },
  component: SignInPage,
});
