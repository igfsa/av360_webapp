import { AlunoDashboard } from "./AlunoDashboard";

export interface GrupoDashboard {
  grupoId: number;
  nome: string;
  media: number;
  totalNotas: number;
  avaliaram: number;
  pendentes: number;

  alunos: AlunoDashboard[];
}
