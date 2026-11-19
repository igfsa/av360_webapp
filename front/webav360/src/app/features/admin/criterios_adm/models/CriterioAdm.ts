export interface CriterioAdm {
  id: number;
  nome: string;
  nativo: boolean;
  professorId: number
  professorNome: string
}

export function convertCriterioAdm(item: any): CriterioAdm {
  return item as CriterioAdm;
}

