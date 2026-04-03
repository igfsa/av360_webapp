import { AvaliacaoPostError } from "./AvaliacaoPostError";

export interface AvaliacaoPostResult {
  total: number;
  sucesso: number;
  falhas: number;
  erros: AvaliacaoPostError[];
}
