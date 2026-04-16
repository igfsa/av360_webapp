import { CriterioDashboard } from './CriterioDashboard';
import { GrupoDashboard } from './GrupoDashboard';

export interface DashboardSessao {
  sessaoId: number;

  totalAlunos: number;
  avaliaram: number;
  pendentes: number;
  mediaGeral: number,
  totalNotas: number,

  criterios: CriterioDashboard[];
  grupos: GrupoDashboard[];
}
