import { Aluno } from "./Aluno";

export interface AvaliacaoAgrupada {
  avaliadoId: number;
  avaliado: Aluno;
  criterios: {
    criterioId: number;
    criterioNome: string;
    nota: number;
  }[];
}
