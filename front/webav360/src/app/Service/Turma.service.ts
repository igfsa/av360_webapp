import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseURL } from '../../main.server';
import { Turma } from '../Models/Turma';

@Injectable({
  providedIn: 'root'
})
export class TurmaService {

  constructor(private http :HttpClient) { }

  public getTurmas (): Observable<Turma[]>{
    return this.http.get<Turma[]>(`${baseURL}api/Turma/GetAllTurmas`);
  }

  public getTurmaId(id: number): Observable<Turma> {
    return this.http.get<Turma>(`${baseURL}api/Turma/GetId/${id}`);
  }

  public getTurmasAluno(id: number): Observable<Turma[]> {
    return this.http.get<Turma[]>(`${baseURL}api/Turma/GetTurmaAlunos/${id}`);
  }

  public putTurma (turma: Turma): Observable<Turma>{
    return this.http.put<Turma>(`${baseURL}api/Turma/Put/${turma.id}`, turma);
  }

  public postTurma (turma: Turma): Observable<Turma>{
    return this.http.post<Turma>(`${baseURL}api/Turma/Post`, turma);
  }

}
