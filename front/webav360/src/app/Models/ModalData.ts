import { AlunoGrupoCheckbox } from "./AlunoGrupoCheckbox";
import { Criterio } from "./Criterio";
import { Grupo } from "./Grupo";
import { Turma } from "./Turma";

export interface AlunoGrupoModalData {
  turma: Turma;
  grupo: Grupo;
  gruposTurma: Grupo[];
  alunosCheck: AlunoGrupoCheckbox[];
}

export interface TurmaGrupoModalData {
  turma: Turma;
  gruposOrig: Grupo[];
}

export interface TurmaGrupoModalOut {
  add: Grupo[],
  edit: Grupo[]
}
export interface TurmaCriterioModalData {
  turma: Turma;
  criterios: Criterio[];
  criteriosTurma: Criterio[];
}
