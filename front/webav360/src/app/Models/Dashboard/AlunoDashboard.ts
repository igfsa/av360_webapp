import { CriterioAlunoDashboard } from './CriterioAlunoDashboard';

export interface AlunoDashboard {
  alunoId: number;
  nome: string;
  media: number;
  grupoId: number,

  criterioAluno: CriterioAlunoDashboard[];

  avaliou: boolean;
  totalNotas: number;
}
