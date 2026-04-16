import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Criterio } from '../Models/Criterio';

@Injectable({
  providedIn: 'root'
})
export class CriterioService {

  constructor(private http :HttpClient) { }

  public getCriterios(): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`/api/Criterio/GetAllCriterios`);
  }

  public getCriterioId(id: number): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`/api/Criterio/GetId/${id}`);
  }

  public getCriteriosTurma(id: number): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`/api/Criterio/GetCriteriosTurma/${id}`);
  }

  public putCriterio (criterio: Criterio): Observable<Criterio>{
    return this.http.put<Criterio>(`/api/Criterio/Put/${criterio.id}`, criterio);
  }

  public postCriterio (criterio: Criterio): Observable<Criterio>{
    return this.http.post<Criterio>(`/api/Criterio/Post`, criterio);
  }

}
