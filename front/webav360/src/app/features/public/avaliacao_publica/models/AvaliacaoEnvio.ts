import { Aluno } from '@shared/models/Aluno';
import { AvaliacaoItem } from './AvaliacaoItem';

export interface AvaliacaoEnvio {
  sessaoId: number,
  grupoId: number,
  avaliadorId: number,
  avaliador: Aluno;
  deviceHash: string,
  comentarioAluno?: string,
  itens: AvaliacaoItem[]
}
