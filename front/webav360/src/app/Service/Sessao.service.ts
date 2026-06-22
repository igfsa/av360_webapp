import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Sessao } from '../Models/Sessao';
import { DashboardSessao } from '../Models/Dashboard/DashboardSessao';
import { SessaoValidacao } from '../Models/SessaoValidacao';
import { Aluno } from '../Models/Aluno';
import { API_URL } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class SessaoService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getSessaoId(id: number): Observable<Sessao> {
    return this.http.get<Sessao>(`${this.baseURL}/api/Sessao/GetSessaoId/${id}`);
  }

  public getSessaoAtivaTurma(turmaId: number): Observable<Sessao> {
    return this.http.get<Sessao>(`${this.baseURL}/api/Sessao/GetSessaoAtivaTurma/${turmaId}`);
  }

  public getSessoesTurmaId(turmaId: number): Observable<Sessao[]> {
    return this.http.get<Sessao[]>(`${this.baseURL}/api/Sessao/GetSessoesTurmaId/${turmaId}`);
  }

  public getExportConsolidado(sessaoId: number): void {
    this.http.get(`${this.baseURL}/api/Sessao/GetExportConsolidado/${sessaoId}`,{responseType: 'blob'})
      .subscribe(blob => {

        const url =
          window.URL.createObjectURL(blob);

        const a =
          document.createElement('a');

        a.href = url;

        a.download = 'avaliacoes.xlsx';

        a.click();

        window.URL.revokeObjectURL(url);
      });
  }

  public getValidaInicioSessao(turmaId: number): Observable<SessaoValidacao> {
    return this.http.get<SessaoValidacao>(`${this.baseURL}/api/Sessao/GetValidaInicioSessao/${turmaId}`);
  }

  public getFaltamAvaliarSessao(sessaoId: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${this.baseURL}/api/Sessao/GetFaltamAvaliarSessao/${sessaoId}`);
  }

  public postSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.post<Sessao>(`${this.baseURL}/api/Sessao/PostSessao`, sessao);
  }

  public putEncerraSessao (sessaoId: number): Observable<Blob>{
    return this.http.put<Blob>(`${this.baseURL}/api/Sessao/PutEncerraSessao/${sessaoId}`,{responseType: 'blob'});
  }

  public dashboardSessao (id: number): Observable<DashboardSessao>{
    return this.http.get<DashboardSessao>(`${this.baseURL}/api/Dashboard/GetDashboard/${id}`);
  }

  public dashboardResetSessao (id: number): Observable<DashboardSessao>{
    return this.http.post<DashboardSessao>(`${this.baseURL}/api/Dashboard/PostDashboardReset/${id}`, id);
  }

  public dashboardResultadoSessao (id: number): Observable<DashboardSessao>{
    return this.http.get<DashboardSessao>(`${this.baseURL}/api/Dashboard/GetResultadoDashboard/${id}`);
  }
}
