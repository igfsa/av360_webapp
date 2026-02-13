import { Grupo } from "./Grupo";
import { Criterio } from "./Criterio";

export interface AvaliacaoPublica {
  sessaoId: number,
  turmaId: number,
  grupos: Grupo[],
  criterios: Criterio[]
}
