import { CriterioDashboard } from './CriterioDashboard';
import { GrupoDashboard } from './GrupoDashboard';

export interface DashboardSessao {
  sessaoId: number;

  totalAlunos: number;
  avaliaram: number;
  pendentes: number;
  mediaGeral: number,
  totalNotas: number,
  notaMax: number
  turmaCod: string,

  criterios: CriterioDashboard[];
  grupos: GrupoDashboard[];
}
