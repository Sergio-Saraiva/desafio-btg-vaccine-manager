import { useRef, useState } from "react";

export function useDebouncedValue<T>(value: T, delayMs: number): T {
  const [debounced, setDebounced] = useState(value);
  const timeoutRef = useRef<ReturnType<typeof setTimeout>>();

  if (debounced !== value) {
    clearTimeout(timeoutRef.current);
    timeoutRef.current = setTimeout(() => setDebounced(value), delayMs);
  }

  return debounced;
}
