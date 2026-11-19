export interface Criterio {
  id: number;
  nome: string;
  nativo: boolean;
}

export function convertCriterio(item: any): Criterio {
  return item as Criterio;
}

