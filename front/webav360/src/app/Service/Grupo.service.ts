import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseURL } from '../../main.server';
import { Grupo } from '../Models/Grupo';

@Injectable({
  providedIn: 'root'
})
export class GrupoService {

  constructor(private http :HttpClient) { }

  public getGrupos (): Observable<Grupo[]>{
    return this.http.get<Grupo[]>(`${baseURL}api/Grupo/GetAllGrupos`);
  }

  public getGrupoId(id: number): Observable<Grupo> {
    return this.http.get<Grupo>(`${baseURL}api/Grupo/GetId/${id}`);
  }

  public getGruposTurma(id: number): Observable<Grupo[]> {
    return this.http.get<Grupo[]>(`${baseURL}api/Grupo/GetGruposTurma/${id}`);
  }

  public postGrupo (grupo: Grupo): Observable<Grupo>{
    return this.http.post<Grupo>(`${baseURL}api/Grupo/Post`, grupo);
  }

  public putGrupo (grupo: Grupo): Observable<Grupo>{
    return this.http.put<Grupo>(`${baseURL}api/Grupo/Put/${grupo.id}`, grupo);
  }

}
