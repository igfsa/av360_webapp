import { Professor } from "./Professor";

export interface TurmaAdm {
    id: number;
    cod: string;
    notaMax: number;
    professorId: number
    professorNome: string
}

export function convertTurmaAdm(item: any): TurmaAdm {
  return item as TurmaAdm;
}
