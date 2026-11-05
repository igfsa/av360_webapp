import { Perfil } from "./Perfil";

export interface Professor {
  id: number;
  userName: string;
  nome: string;
  senhaHash: string;
  perfil: Perfil;
}

export function convertProfessor(item: any): Professor {
  return item as Professor;
}
