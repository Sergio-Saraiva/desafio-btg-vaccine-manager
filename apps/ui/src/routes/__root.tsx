import { createRootRoute, Link, Outlet, useRouter } from "@tanstack/react-router";
import { useQueryClient } from "@tanstack/react-query";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { isAuthenticated, removeToken } from "@/lib/auth";
import { LogOut } from "lucide-react";

function RootLayout() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const authenticated = isAuthenticated();

  const handleLogout = () => {
    removeToken();
    queryClient.clear();
    router.invalidate().then(() => {
      router.navigate({ to: "/sign-in" });
    });
  };

  if (!authenticated) {
    return <Outlet />;
  }

  return (
    <div className="min-h-screen bg-background">
      <header className="border-b">
        <div className="container mx-auto flex h-14 items-center gap-6 px-6">
          <span className="text-lg font-semibold">Vaccine Manager</span>
          <nav className="flex gap-4">
            <Link
              to="/persons"
              className="text-sm text-muted-foreground hover:text-foreground [&.active]:text-foreground [&.active]:font-medium"
            >
              Persons
            </Link>
            <Link
              to="/vaccines"
              className="text-sm text-muted-foreground hover:text-foreground [&.active]:text-foreground [&.active]:font-medium"
            >
              Vaccines
            </Link>
          </nav>
          <div className="ml-auto">
            <Button variant="ghost" size="sm" onClick={handleLogout}>
              <LogOut className="size-4 mr-1" />
              Logout
            </Button>
          </div>
        </div>
      </header>
      <Separator />
      <main className="container mx-auto px-6 py-8">
        <Outlet />
      </main>
    </div>
  );
}

export const Route = createRootRoute({
  component: RootLayout,
});
