import { ImportAlunosError } from "./TurmaImportError";

export interface ImportAlunosResult {
  total: number;
  sucesso: number;
  falhas: number;
  erros: ImportAlunosError[];
}
