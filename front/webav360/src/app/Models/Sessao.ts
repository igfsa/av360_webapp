export interface Sessao {
  id: number;
  turmaId: number;
  dataInicio: Date;
  dataFim: Date;
  tokenPublico: string;
  ativo: boolean;
}
