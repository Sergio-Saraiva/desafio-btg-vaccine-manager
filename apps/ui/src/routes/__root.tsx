import { createRootRoute, Link, Outlet } from "@tanstack/react-router";
  import { Separator } from "@/components/ui/separator";                                                                      
                                                                                                                              
  export const Route = createRootRoute({                                                                                      
    component: () => (                                                                                                        
      <div className="min-h-screen bg-background">
        <header className="border-b">                                                                                         
          <div className="container mx-auto flex h-14 items-center gap-6 px-6">
            <span className="text-lg font-semibold">Vaccine Manager</span>                                                    
            <nav className="flex gap-4">                                                                                      
              <Link                                                                                                           
                to="/persons"                                                                                                 
                className="text-sm text-muted-foreground hover:text-foreground [&.active]:text-foreground                     
  [&.active]:font-medium"                                                                                                     
              >                                                                                                               
                Persons                                                                                                       
              </Link>
            </nav>
          </div>
        </header>
        <Separator />
        <main className="container mx-auto px-6 py-8">
          <Outlet />
        </main>                                                                                                               
      </div>                                                                                                                  
    ),                                                                                                                        
  });      