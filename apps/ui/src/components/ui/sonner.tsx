import { Toaster as Sonner } from "sonner";

function Toaster() {
  return (
    <Sonner
      richColors
      position="bottom-right"
      toastOptions={{
        classNames: {
          toast: "font-sans",
        },
      }}
    />
  );
}

export { Toaster };
