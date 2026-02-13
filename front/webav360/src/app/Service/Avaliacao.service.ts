import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseURL } from '../../main.server';
import { AvaliacaoPublica } from '../Models/AvaliacaoPublica';
import { AvaliacaoEnvio } from '../Models/AvaliacaoEnvio';

@Injectable({
  providedIn: 'root'
})
export class AvaliacaoService {

  constructor(private http :HttpClient) { }

  public GetValidaSessaoChavePub(token: string): Observable<AvaliacaoPublica> {
    return this.http.get<AvaliacaoPublica>(`${baseURL}api/Avaliacao/GetValidaSessaoChavePub`,  {params: {token}});
  }

  public GeraNovaAvaliacaoEnvio(sessaoId: number, grupoId: number, avaliadorId: number): Observable<AvaliacaoEnvio> {
    const params = new HttpParams()
      .set('sessaoId', sessaoId)
      .set('grupoId', grupoId)
      .set('avaliadorId', avaliadorId);
    return this.http.get<AvaliacaoEnvio>(`${baseURL}api/Avaliacao/GeraNovaAvaliacaoEnvio`, {params});
  }

  public PostAvaliacao(avaliacao: AvaliacaoEnvio){
    return this.http.post(`${baseURL}api/Avaliacao/Post`,  avaliacao);
  }
}
