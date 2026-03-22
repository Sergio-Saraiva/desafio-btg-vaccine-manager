import { PersonDetailsPage } from "@/pages/Persons/person-details-page";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute("/persons/$personId")({
  component: RouteComponent,
});

function RouteComponent() {
  const { personId } = Route.useParams();
  return <PersonDetailsPage personId={personId} />;
}
