import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../app.config';
import { Professor } from '../Models/Professor';

@Injectable({
  providedIn: 'root'
})
export class ProfessorService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getProfessores(): Observable<Professor[]> {
    return this.http.get<Professor[]>(`${this.baseURL}/api/Professor/GetAllProfessores`);
  }

  public postProfessor (professor: Professor): Observable<Professor>{
    return this.http.post<Professor>(`${this.baseURL}/api/Autenticacao/Register`, professor);
  }

}
