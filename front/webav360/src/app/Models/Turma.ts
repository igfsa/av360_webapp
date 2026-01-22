import { Aluno } from "./Aluno";
import { Criterio } from "./Criterio";

export interface Turma {
    id: number;
    cod: string;
    notaMax: number;
    // alunos: Aluno[];
    // criterios: Criterio[];
}

export function createEmptyTurma(): Turma {
  return {
    id: 0,
    cod: '',
    notaMax: 0
  }
}
