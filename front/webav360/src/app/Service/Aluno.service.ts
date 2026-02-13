import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Aluno } from '../Models/Aluno';
import { Observable } from 'rxjs';
import { _GlobalVariablesService } from './_GlobalVariables.service';
import { baseURL } from '../../main.server';
import { AlunoGrupoNomes } from '../Models/AlunoGrupoNomes';

@Injectable({
  providedIn: 'root'
})
export class AlunoService {

  constructor(private http :HttpClient) { }

  public getAlunos(): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}api/Aluno/GetAllAlunos`);
  }

  public getAlunoId(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}api/Aluno/GetId/${id}`);
  }

  public getAlunosTurma(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}api/Aluno/GetAlunosTurma/${id}`);
  }

  public getAlunoNomeIdGrupo(id: number, nome: string): Observable<Aluno> {
    return this.http.get<Aluno>(`${baseURL}api/Aluno/ObterAlunoNomeIdGrupo/${id}`, {params: {nome: nome}});
  }

  public getAlunoGrupoNome(id: number): Observable<AlunoGrupoNomes[]> {
    return this.http.get<AlunoGrupoNomes[]>(`${baseURL}api/Aluno/GetAlunoGrupoNome/${id}`);
  }

  public getAlunosGrupo(grupoId: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}api/Aluno/GetAlunosGrupo/${grupoId}`);
  }
}

