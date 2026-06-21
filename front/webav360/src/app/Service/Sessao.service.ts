import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Sessao } from '../Models/Sessao';
import { DashboardSessao } from '../Models/Dashboard/DashboardSessao';
import { SessaoValidacao } from '../Models/SessaoValidacao';
import { Aluno } from '../Models/Aluno';
import { baseURL } from '../../main.server';

@Injectable({
  providedIn: 'root'
})
export class SessaoService {

  constructor(private http :HttpClient) { }

  public getSessaoId(id: number): Observable<Sessao> {
    return this.http.get<Sessao>(`${baseURL}/api/Sessao/GetSessaoId/${id}`);
  }

  public getSessaoAtivaTurma(turmaId: number): Observable<Sessao> {
    return this.http.get<Sessao>(`${baseURL}/api/Sessao/GetSessaoAtivaTurma/${turmaId}`);
  }

  public getSessoesTurmaId(turmaId: number): Observable<Sessao[]> {
    return this.http.get<Sessao[]>(`${baseURL}/api/Sessao/GetSessoesTurmaId/${turmaId}`);
  }

  public getExportConsolidado(sessaoId: number): void {
    this.http.get(`${baseURL}/api/Sessao/GetExportConsolidado/${sessaoId}`,{responseType: 'blob'})
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
    return this.http.get<SessaoValidacao>(`${baseURL}/api/Sessao/GetValidaInicioSessao/${turmaId}`);
  }

  public getFaltamAvaliarSessao(sessaoId: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}/api/Sessao/GetFaltamAvaliarSessao/${sessaoId}`);
  }

  public postSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.post<Sessao>(`${baseURL}/api/Sessao/PostSessao`, sessao);
  }

  public putEncerraSessao (sessaoId: number): Observable<Blob>{
    return this.http.put<Blob>(`${baseURL}/api/Sessao/PutEncerraSessao/${sessaoId}`,{responseType: 'blob'});
  }

  public dashboardSessao (id: number): Observable<DashboardSessao>{
    return this.http.get<DashboardSessao>(`${baseURL}/api/Dashboard/GetDashboard/${id}`);
  }

  public dashboardResetSessao (id: number): Observable<DashboardSessao>{
    return this.http.post<DashboardSessao>(`${baseURL}/api/Dashboard/PostDashboardReset/${id}`, id);
  }

  public dashboardResultadoSessao (id: number): Observable<DashboardSessao>{
    return this.http.get<DashboardSessao>(`${baseURL}/api/Dashboard/GetResultadoDashboard/${id}`);
  }
}
