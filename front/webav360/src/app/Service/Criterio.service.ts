import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Criterio } from '../Models/Criterio';
import { API_URL } from '../app.config';

@Injectable({
  providedIn: 'root'
})
export class CriterioService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getCriterios(): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${this.baseURL}/api/Criterio/GetAllCriterios`);
  }

  public getCriteriosTurma(id: number): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${this.baseURL}/api/Criterio/GetCriteriosTurma/${id}`);
  }

  public putCriterio (criterio: Criterio): Observable<Criterio>{
    return this.http.put<Criterio>(`${this.baseURL}/api/Criterio/PutCriterio/${criterio.id}`, criterio);
  }

  public postCriterio (criterio: Criterio): Observable<Criterio>{
    return this.http.post<Criterio>(`${this.baseURL}/api/Criterio/PostCriterio`, criterio);
  }

}
