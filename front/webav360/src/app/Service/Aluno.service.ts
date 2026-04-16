import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Aluno } from '../Models/Aluno';
import { Observable } from 'rxjs';
import { _GlobalVariablesService } from './_GlobalVariables.service';
import { AlunoGrupoNomes } from '../Models/AlunoGrupoNomes';

@Injectable({
  providedIn: 'root'
})
export class AlunoService {

  constructor(private http :HttpClient) { }

  public getAlunos(): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`/api/Aluno/GetAllAlunos`);
  }

  public getAlunoId(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`/api/Aluno/GetId/${id}`);
  }

  public getAlunosTurma(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`/api/Aluno/GetAlunosTurma/${id}`);
  }

  public getAlunoNomeIdGrupo(id: number, nome: string): Observable<Aluno> {
    return this.http.get<Aluno>(`/api/Aluno/ObterAlunoNomeIdGrupo/${id}`, {params: {nome: nome}});
  }

  public getAlunoGrupoNome(id: number): Observable<AlunoGrupoNomes[]> {
    return this.http.get<AlunoGrupoNomes[]>(`/api/Aluno/GetAlunoGrupoNome/${id}`);
  }

  public getAlunosGrupo(grupoId: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`/api/Aluno/GetAlunosGrupo/${grupoId}`);
  }
}

