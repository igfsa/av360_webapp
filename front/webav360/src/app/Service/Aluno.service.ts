import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Aluno } from '../Models/Aluno';
import { Observable } from 'rxjs';
import { _GlobalVariablesService } from './_GlobalVariables.service';
import { baseURL } from '../../main.server';

@Injectable({
  providedIn: 'root'
})
export class AlunoService {

  constructor(private http :HttpClient) { }

  public getAlunos(): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}Aluno/GetAllAlunos`);
  }

  public getAlunoId(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}Aluno/GetId/${id}`);
  }

  public getAlunosTurma(id: number): Observable<Aluno[]> {
    return this.http.get<Aluno[]>(`${baseURL}Aluno/GetAlunosTurma/${id}`);
  }
}

