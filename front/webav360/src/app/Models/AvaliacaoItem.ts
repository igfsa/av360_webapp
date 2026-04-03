import { Aluno } from "./Aluno"

export interface AvaliacaoItem {
  avaliadoId: number,
  avaliado: Aluno,
  criterioId: number,
  nota: number
}
