import { PersonsPage } from "@/pages/Persons/persons-page";
import { createFileRoute } from "@tanstack/react-router";
                  
  export const Route = createFileRoute("/persons")({                                                                          
    component: PersonsPage,
  }); 