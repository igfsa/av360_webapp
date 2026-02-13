import { AvaliacaoItem } from "./AvaliacaoItem";

export interface AvaliacaoEnvio {
  sessaoId: number,
  grupoId: number,
  avaliadorId: number,
  deviceHash: string,
  itens: AvaliacaoItem[]
}
