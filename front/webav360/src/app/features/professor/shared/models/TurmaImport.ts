import { File } from "buffer";

export interface ImportAlunos {
  turmaId: number;
  colunaNome: string;
  file: File | null;
}
