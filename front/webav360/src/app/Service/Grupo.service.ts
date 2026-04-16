import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Grupo } from '../Models/Grupo';
import { AlunoGrupoCheckbox } from '../Models/AlunoGrupoCheckbox';
import { AlunoGrupo } from '../Models/AlunoGrupo';

@Injectable({
  providedIn: 'root'
})
export class GrupoService {

  constructor(private http :HttpClient) { }

  public getGrupos (): Observable<Grupo[]>{
    return this.http.get<Grupo[]>(`/api/Grupo/GetAllGrupos`);
  }

  public getGrupoId(id: number): Observable<Grupo> {
    return this.http.get<Grupo>(`/api/Grupo/GetId/${id}`);
  }

  public getGruposTurma(id: number): Observable<Grupo[]> {
    return this.http.get<Grupo[]>(`/api/Grupo/GetGruposTurma/${id}`);
  }

  public GetAlunoGruposCheckbox(grupoId: number, turmaId: number): Observable<AlunoGrupoCheckbox[]> {
    return this.http.get<AlunoGrupoCheckbox[]>(`/api/Grupo/GetAlunoGruposCheckbox/${turmaId}`, {params: {turmaId, grupoId}});
  }

  public postGrupo (grupo: Grupo): Observable<Grupo>{
    return this.http.post<Grupo>(`/api/Grupo/Post`, grupo);
  }

  public putGrupo (grupo: Grupo): Observable<Grupo>{
    return this.http.put<Grupo>(`/api/Grupo/Put/${grupo.id}`, grupo);
  }

  public putAtualizarGrupo (alunosGrupo: AlunoGrupo){
    return this.http.put<AlunoGrupo>(`/api/Grupo/PutAtualizarGrupo/`, alunosGrupo);
  }

}
