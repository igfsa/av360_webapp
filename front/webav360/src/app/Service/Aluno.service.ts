import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Aluno } from '../Models/Aluno';
import { Observable } from 'rxjs';
import { _GlobalVariablesService } from './_GlobalVariables.service';
import { AlunoGrupoNomes } from '../Models/AlunoGrupoNomes';
import { baseURL } from '../../main.server';

@Injectable({
  providedIn: 'root'
})
export class AlunoService {

  constructor(private http :HttpClient) { }

  public getAlunosTurma(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}/api/Aluno/GetAlunosTurma/${id}`);
  }

  public getAlunoNomeIdGrupo(id: number, nome: string): Observable<Aluno> {
    return this.http.get<Aluno>(`${baseURL}/api/Aluno/GetAlunoNomeIdGrupo/${id}`, {params: {nome: nome}});
  }

  public postAlunoTurma (turmaId: number, aluno: Aluno): Observable<Aluno>{
    return this.http.post<Aluno>(`${baseURL}/api/Aluno/PostAlunoTurma/${turmaId}`, aluno);
  }

  public getAlunoGrupoNome(id: number): Observable<AlunoGrupoNomes[]> {
    return this.http.get<AlunoGrupoNomes[]>(`${baseURL}/api/Aluno/GetAlunoGrupoNome/${id}`);
  }
}

