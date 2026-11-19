import { Grupo } from '@shared/models/Grupo';
import { Criterio } from '@shared/models/Criterio';
import { Turma } from '@shared/models/Turma';

export interface AvaliacaoPublica {
  sessaoId: number,
  turmaId: number,
  turma: Turma;
  grupos: Grupo[],
  criterios: Criterio[]
}
