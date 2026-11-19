import { Aluno } from '@shared/models/Aluno'

export interface AvaliacaoItem {
  avaliadoId: number,
  avaliado: Aluno,
  criterioId: number,
  nota: number
}
