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
    return this.http.get<Turma[]>(`${baseURL}Turma/GetAllTurmas`);
  }

  public getTurmaId(id: number): Observable<Turma> {
    return this.http.get<Turma>(`${baseURL}Turma/GetId/${id}`);
  }

  public getTurmasAluno(id: number): Observable<Turma[]> {
    return this.http.get<Turma[]>(`${baseURL}Turma/GetTurmaAlunos/${id}`);
  }
}
