import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Criterio } from '../Models/Criterio';
import { baseURL } from '../../main.server';

@Injectable({
  providedIn: 'root'
})
export class CriterioService {

  constructor(private http :HttpClient) { }

  public getCriterios(): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${baseURL}api/Criterio/GetAllCriterios`);
  }

  public getCriterioId(id: number): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${baseURL}api/Criterio/GetId/${id}`);
  }

  public getCriteriosTurma(id: number): Observable<Criterio[]> {
    return this.http.get<Criterio[]>(`${baseURL}api/Criterio/GetCriteriosTurma/${id}`);
  }

}
