import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class ResultadoService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public putEncerraSessao (sessaoId: number): Observable<Blob>{
    return this.http.put(`${this.baseURL}/api/Resultado/RecalcularResultadoSessao/${sessaoId}`,null,{responseType: `blob`});
  }

}
