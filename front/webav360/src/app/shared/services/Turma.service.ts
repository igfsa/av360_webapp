import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Turma } from '@shared/models/Turma';
import { TurmaCriterio } from '@app/features/professor/turmas/models/TurmaCriterio';
import { ImportAlunos } from '@app/features/professor/shared/models/TurmaImport';
import { ImportAlunosResult } from '@app/features/professor/shared/models/TurmaImportResult';
import { API_URL } from '@core/config/app.config';
import { TurmaAdm } from '@app/features/admin/turmas_adm/models/TurmaAdm';

@Injectable({
  providedIn: 'root'
})
export class TurmaService {

  constructor(private http :HttpClient, @Inject(API_URL) public readonly baseURL: string) { }

  public getTurmasAdm (): Observable<TurmaAdm[]>{
    return this.http.get<TurmaAdm[]>(`${this.baseURL}/api/Turma/GetAllTurmasAdm`);
  }

  public getTurmaId(id: number): Observable<Turma> {
    return this.http.get<Turma>(`${this.baseURL}/api/Turma/GetTurmaById/${id}`);
  }

  public getTurmasProfessorId(): Observable<Turma[]> {
    return this.http.get<Turma[]>(`${this.baseURL}/api/Turma/GetTurmasProfessorId/`);
  }

  public postTurma (turma: Turma): Observable<Turma>{
    return this.http.post<Turma>(`${this.baseURL}/api/Turma/PostTurma`, turma);
  }

  public putTurma (turma: Turma): Observable<Turma>{
    return this.http.put<Turma>(`${this.baseURL}/api/Turma/PutTurma/${turma.id}`, turma);
  }

  public putCriterioTurma (turmaCriterio: TurmaCriterio): Observable<Turma>{
    return this.http.put<Turma>(`${this.baseURL}/api/Turma/PutCriterioTurma`, turmaCriterio);
  }

  public putAlterarProfessor (turmaId: number, professorId: number): Observable<TurmaAdm>{
    return this.http.put<TurmaAdm>(`${this.baseURL}/api/Turma/AlterarProfessor/${turmaId}?professorId=${professorId}`, {});
  }

  public putRemoverProfessor (turmaId: number): Observable<Turma>{
    return this.http.put<Turma>(`${this.baseURL}/api/Turma/RemoverProfessor/${turmaId}`, {});
  }

  public postImportarAlunos (importAlunos: ImportAlunos): Observable<ImportAlunosResult>{
    const formData = new FormData();
    formData.append('TurmaId', importAlunos.turmaId.toString());
    formData.append('ColunaNome', importAlunos.colunaNome);
    formData.append('Arquivo', importAlunos.file as File);
    return this.http.post<ImportAlunosResult>(`${this.baseURL}/api/Turma/PostImportAlunosTurma`, formData);
  }
}
