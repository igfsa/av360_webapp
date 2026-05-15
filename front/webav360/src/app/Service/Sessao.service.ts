import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Sessao } from '../Models/Sessao';
import { DashboardSessao } from '../Models/Dashboard/DashboardSessao';
import { SessaoValidacao } from '../Models/SessaoValidacao';
import { Aluno } from '../Models/Aluno';

@Injectable({
  providedIn: 'root'
})
export class SessaoService {

  constructor(private http :HttpClient) { }

  public getSessaoId(id: number): Observable<Sessao> {
    return this.http.get<Sessao>(`/api/Sessao/GetSessaoId/${id}`);
  }

  public getSessaoAtivaTurma(turmaId: number): Observable<Sessao> {
    return this.http.get<Sessao>(`/api/Sessao/GetSessaoAtivaTurma/${turmaId}`);
  }

  public getSessoesTurmaId(turmaId: number): Observable<Sessao> {
    return this.http.get<Sessao>(`/api/Sessao/GetSessoesTurmaId/${turmaId}`);
  }

  public getExportConsolidado(sessaoId: number): void {
    this.http.get(`/api/Sessao/GetExportConsolidado/${sessaoId}`,{responseType: 'blob'})
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
    return this.http.get<SessaoValidacao>(`/api/Sessao/GetValidaInicioSessao/${turmaId}`);
  }

  public getFaltamAvaliarSessao(sessaoId: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`/api/Sessao/GetFaltamAvaliarSessao/${sessaoId}`);
  }

  public postSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.post<Sessao>(`/api/Sessao/PostSessao`, sessao);
  }

  public putEncerraSessao (sessao: Sessao): Observable<Sessao>{
    return this.http.put<Sessao>(`/api/Sessao/PutEncerraSessao/${sessao.id}`, sessao);
  }

  public dashboardSessao (id: number): Observable<DashboardSessao>{
    return this.http.get<DashboardSessao>(`/api/Dashboard/GetDashboard/${id}`);
  }

  public dashboardResetSessao (id: number): Observable<DashboardSessao>{
    return this.http.post<DashboardSessao>(`/api/Dashboard/PostDashboardReset/${id}`, id);
  }
}
