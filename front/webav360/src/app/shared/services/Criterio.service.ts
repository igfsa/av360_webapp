import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Criterio } from '@shared/models/Criterio';
import { API_URL } from '@core/config/app.config';
import { CriterioAdm } from '@app/features/admin/criterios_adm/models/CriterioAdm';

@Injectable({
  providedIn: 'root'
})
export class CriterioService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getCriteriosAdm(): Observable<CriterioAdm[]> {
    return this.http.get<CriterioAdm[]>(`${this.baseURL}/api/Criterio/GetAllCriteriosAdm`);
  }

  public getCriteriosTurma(id: number): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${this.baseURL}/api/Criterio/GetCriteriosTurma/${id}`);
  }

  public getCriteriosProfessor(): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${this.baseURL}/api/Criterio/GetCriteriosProfessor`);
  }

  public getCriteriosSugestoes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseURL}/api/Criterio/getCriteriosSugestoes`);
  }

  public putCriterio (criterio: Criterio): Observable<Criterio>{
    return this.http.put<Criterio>(`${this.baseURL}/api/Criterio/PutCriterio/${criterio.id}`, criterio);
  }


  public putCriterioAdm (criterio: Criterio): Observable<Criterio>{
    return this.http.put<Criterio>(`${this.baseURL}/api/Criterio/PutCriterioAdm/${criterio.id}`, criterio);
  }

  public postCriterio (criterio: Criterio): Observable<Criterio>{
    return this.http.post<Criterio>(`${this.baseURL}/api/Criterio/PostCriterio`, criterio);
  }

  public postCriterioAdm (criterio: Criterio): Observable<Criterio>{
    return this.http.post<Criterio>(`${this.baseURL}/api/Criterio/PostCriterioAdm`, criterio);
  }

}
