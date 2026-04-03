import { Grupo } from "./Grupo";
import { Criterio } from "./Criterio";
import { Turma } from "./Turma";

export interface AvaliacaoPublica {
  sessaoId: number,
  turmaId: number,
  turma: Turma;
  grupos: Grupo[],
  criterios: Criterio[]
}
