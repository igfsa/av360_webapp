import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Sessao } from '../Models/Sessao';
import { DashboardSessao } from '../Models/Dashboard/DashboardSessao';

@Injectable({
  providedIn: 'root'
})
export class SessaoService {

  constructor(private http :HttpClient) { }

  public getSessaos (): Observable<Sessao[]>{
    return this.http.get<Sessao[]>(`/api/Sessao/GetAllSessoes`);
  }

  public getSessaoId(id: number): Observable<Sessao> {
    return this.http.get<Sessao>(`/api/Sessao/GetId/${id}`);
  }

  public GetSessoesTurma(turmaId: number): Observable<Sessao[]> {
    return this.http.get<Sessao[]>(`/api/Sessao/GetSessoesTurma/${turmaId}`);
  }

  public GetSessaoAtivaTurma(turmaId: number): Observable<Sessao> {
    return this.http.get<Sessao>(`/api/Sessao/GetSessaoAtivaTurma/${turmaId}`);
  }

  public postSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.post<Sessao>(`/api/Sessao/Post`, sessao);
  }

  public putSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.put<Sessao>(`/api/Sessao/Put/${sessao.id}`, sessao);
  }

  public putEncerraSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.put<Sessao>(`/api/Sessao/PutEncerra/${sessao.id}`, sessao);
  }

  public dashboardSessao (id: number): Observable<DashboardSessao>{
    return this.http.get<DashboardSessao>(`/api/Dashboard/GetDashboard/${id}`);
  }

  public dashboardResetSessao (id: number): Observable<DashboardSessao>{
    return this.http.post<DashboardSessao>(`/api/Dashboard/Reset/${id}`, id);
  }
}
