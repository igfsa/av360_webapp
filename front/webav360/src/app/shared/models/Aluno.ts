export interface Aluno {
  id : number;
  nome : string;
}

export function convertAluno(item: any): Aluno {
  return item as Aluno;
}
