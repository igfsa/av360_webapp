export interface AvaliacaoAgrupada {
  avaliadoId: number;
  avaliadoNome: string;
  criterios: {
    criterioId: number;
    criterioNome: string;
    nota: number;
  }[];
}
