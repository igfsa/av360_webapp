export interface Grupo {
    id: number;
    nome: string;
    turmaId: number;
}

export function createEmptyGrupo(): Grupo {
  return {
    id: 0,
    nome: '',
    turmaId: 0
  }
}
