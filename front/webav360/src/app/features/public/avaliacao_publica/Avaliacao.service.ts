import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AvaliacaoPublica } from './models/AvaliacaoPublica';
import { AvaliacaoEnvio } from './models/AvaliacaoEnvio';
import { AvaliacaoPostResult } from './models/AvaliacaoPostResult';
import { API_URL } from '@core/config/app.config';

@Injectable({
  providedIn: 'root'
})
export class AvaliacaoService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getValidaSessaoChavePub(token: string): Observable<AvaliacaoPublica> {
    return this.http.get<AvaliacaoPublica>(`${this.baseURL}/api/Avaliacao/GetValidaSessaoChavePub`,  {params: {token}});
  }

  public getNovaAvaliacaoEnvio(sessaoId: number, grupoId: number, avaliadorId: number, hash: string): Observable<AvaliacaoEnvio> {
    const params = new HttpParams()
      .set('sessaoId', sessaoId)
      .set('grupoId', grupoId)
      .set('avaliadorId', avaliadorId)
      .set(`deviceHash`, hash);
    return this.http.get<AvaliacaoEnvio>(`${this.baseURL}/api/Avaliacao/GetNovaAvaliacaoEnvio`, {params});
  }

  public postAvaliacao(avaliacao: AvaliacaoEnvio): Observable<AvaliacaoPostResult>{
    return this.http.post<AvaliacaoPostResult>(`${this.baseURL}/api/Avaliacao/PostAvaliacao`,  avaliacao);
  }
}
