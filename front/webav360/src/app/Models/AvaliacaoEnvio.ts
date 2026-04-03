import { Aluno } from "./Aluno";
import { AvaliacaoItem } from "./AvaliacaoItem";

export interface AvaliacaoEnvio {
  sessaoId: number,
  grupoId: number,
  avaliadorId: number,
  avaliador: Aluno;
  deviceHash: string,
  itens: AvaliacaoItem[]
}
