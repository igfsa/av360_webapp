import { Perfil } from "./Perfil";

export interface ProfessorRegister {
  id: number;
  userName: string;
  nome: string;
  senhaHash: string;
  perfil: Perfil;
}

export function convertProfessor(item: any): ProfessorRegister {
  return item as ProfessorRegister;
}
