import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { baseURL } from '../../main.server';
import { Turma } from '../Models/Turma';
import { TurmaCriterio } from '../Models/TurmaCriterio';
import { ImportAlunos } from '../Models/TurmaImport';
import { ImportAlunosResult } from '../Models/TurmaImportResult';

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

  public postTurma (turma: Turma): Observable<Turma>{
    return this.http.post<Turma>(`${baseURL}api/Turma/Post`, turma);
  }

  public putTurma (turma: Turma): Observable<Turma>{
    return this.http.put<Turma>(`${baseURL}api/Turma/Put/${turma.id}`, turma);
  }

  public putCriterioTurma (turmaCriterio: TurmaCriterio): Observable<Turma>{
    return this.http.put<Turma>(`${baseURL}api/Turma/PutCriterioTurma/`, turmaCriterio);
  }
  public postImportarAlunos (importAlunos: ImportAlunos): Observable<ImportAlunosResult>{
    console.log(importAlunos);
    const formData = new FormData();
    formData.append('TurmaId', importAlunos.turmaId.toString());
    formData.append('ColunaNome', importAlunos.colunaNome);
    formData.append('Arquivo', importAlunos.file as File);
    console.log(formData);
    return this.http.post<ImportAlunosResult>(`${baseURL}api/Turma/ImportAlunosTurma/${importAlunos.turmaId}`, formData);
  }
}
