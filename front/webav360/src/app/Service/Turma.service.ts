import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Turma } from '../Models/Turma';
import { TurmaCriterio } from '../Models/TurmaCriterio';
import { ImportAlunos } from '../Models/TurmaImport';
import { ImportAlunosResult } from '../Models/TurmaImportResult';
import { Aluno } from '../Models/Aluno';

@Injectable({
  providedIn: 'root'
})
export class TurmaService {

  constructor(private http :HttpClient) { }

  public getTurmas (): Observable<Turma[]>{
    return this.http.get<Turma[]>(`/api/Turma/GetAllTurmas`);
  }

  public getTurmaId(id: number): Observable<Turma> {
    return this.http.get<Turma>(`/api/Turma/GetTurmaById/${id}`);
  }

  public postTurma (turma: Turma): Observable<Turma>{
    return this.http.post<Turma>(`/api/Turma/PostTurma`, turma);
  }

  public putTurma (turma: Turma): Observable<Turma>{
    return this.http.put<Turma>(`/api/Turma/PutTurma/${turma.id}`, turma);
  }

  public putCriterioTurma (turmaCriterio: TurmaCriterio): Observable<Turma>{
    return this.http.put<Turma>(`/api/Turma/PutCriterioTurma`, turmaCriterio);
  }

  public postImportarAlunos (importAlunos: ImportAlunos): Observable<ImportAlunosResult>{
    const formData = new FormData();
    formData.append('TurmaId', importAlunos.turmaId.toString());
    formData.append('ColunaNome', importAlunos.colunaNome);
    formData.append('Arquivo', importAlunos.file as File);
    return this.http.post<ImportAlunosResult>(`/api/Turma/PostImportAlunosTurma`, formData);
  }
}
