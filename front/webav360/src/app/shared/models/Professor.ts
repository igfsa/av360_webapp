import { Perfil } from "@app/core/auth/models/Perfil";

export interface Professor {
  id: number;
  nome: string;
  userName: string;
  perfil: Perfil;
}

export function convertProfessor(item: any): Professor {
  return item as Professor;
}
