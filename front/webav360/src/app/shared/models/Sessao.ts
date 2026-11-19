import { Turma } from "./Turma";

export interface Sessao {
  id: number;
  turmaId: number;
  turma: Turma;
  dataInicio: Date;
  dataFim: Date;
  tokenPublico: string;
  ativo: boolean;
}
