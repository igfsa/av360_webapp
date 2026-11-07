import { AlunoGrupoCheckbox } from "./AlunoGrupoCheckbox";
import { Criterio } from "./Criterio";
import { Grupo } from "./Grupo";
import { Professor } from "./Professor";
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
  add: Grupo[];
  edit: Grupo[];
}
export interface TurmaCriterioModalData {
  turma: Turma;
  criterios: Criterio[];
  criteriosTurma: Criterio[];
}

export interface TurmaCriarModalData {
  turma: Turma;
  importarAlunos: boolean;
}


export interface TurmaProfessorModalData {
  turma: Turma;
  professor: Professor | null;
  professores: Professor[];
}
