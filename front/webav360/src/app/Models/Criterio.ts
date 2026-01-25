import { Turma } from "./Turma";

export interface Criterio {
  id: number;
  nome: string;
  // turmas: Turma[];
}

export function createEmptyCriterio(): Criterio {
  return {
    id: 0,
    nome: ``
  }}
