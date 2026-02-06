import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseURL } from '../../main.server';
import { Sessao } from '../Models/Sessao';

@Injectable({
  providedIn: 'root'
})
export class SessaoService {

  constructor(private http :HttpClient) { }

  public getSessaos (): Observable<Sessao[]>{
    return this.http.get<Sessao[]>(`${baseURL}api/Sessao/GetAllSessoes`);
  }

  public getSessaoId(id: number): Observable<Sessao> {
    return this.http.get<Sessao>(`${baseURL}api/Sessao/GetId/${id}`);
  }

  public GetSessoesTurma(turmaId: number): Observable<Sessao[]> {
    return this.http.get<Sessao[]>(`${baseURL}api/Sessao/GetSessoesTurma/${turmaId}`);
  }

  public GetSessaoAtivaTurma(turmaId: number): Observable<Sessao> {
    return this.http.get<Sessao>(`${baseURL}api/Sessao/GetSessaoAtivaTurma/${turmaId}`);
  }

  public postSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.post<Sessao>(`${baseURL}api/Sessao/Post`, sessao);
  }

  public putSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.put<Sessao>(`${baseURL}api/Sessao/Put/${sessao.id}`, sessao);
  }

  public putEncerraSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.put<Sessao>(`${baseURL}api/Sessao/PutEncerra/${sessao.id}`, sessao);
  }
}
