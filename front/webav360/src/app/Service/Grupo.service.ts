import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Grupo } from '../Models/Grupo';
import { AlunoGrupoCheckbox } from '../Models/AlunoGrupoCheckbox';
import { AlunoGrupo } from '../Models/AlunoGrupo';
import { API_URL } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class GrupoService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getGruposTurma(id: number): Observable<Grupo[]> {
    return this.http.get<Grupo[]>(`${this.baseURL}/api/Grupo/GetGruposTurma/${id}`);
  }

  public getAlunoGruposCheckbox(grupoId: number, turmaId: number): Observable<AlunoGrupoCheckbox[]> {
    return this.http.get<AlunoGrupoCheckbox[]>(`${this.baseURL}/api/Grupo/GetAlunoGruposCheckbox/${turmaId}`, {params: {turmaId, grupoId}});
  }

  public postGrupo (grupo: Grupo): Observable<Grupo>{
    return this.http.post<Grupo>(`${this.baseURL}/api/Grupo/PostGrupo`, grupo);
  }

  public putGrupo (grupo: Grupo): Observable<Grupo>{
    return this.http.put<Grupo>(`${this.baseURL}/api/Grupo/PutGrupo/${grupo.id}`, grupo);
  }

  public putAtualizarGrupo (alunosGrupo: AlunoGrupo){
    return this.http.put<AlunoGrupo>(`${this.baseURL}/api/Grupo/PutAtualizarGrupo/`, alunosGrupo);
  }

}
