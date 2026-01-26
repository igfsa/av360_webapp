import { File } from "buffer";

export interface ImportAlunos {
  turmaId: number;
  colunaNome: string;
  file: File | null;
}

export function createEmptyImport(): ImportAlunos {
  return {
    turmaId: 0,
    colunaNome: '',
    file: null
  }
}
