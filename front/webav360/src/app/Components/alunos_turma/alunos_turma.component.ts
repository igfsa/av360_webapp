import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, ActivatedRoute } from '@angular/router';

import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';
import { TurmaService } from '../../Service/Turma.service';
import { Turma } from '../../Models/Turma';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';
import { forkJoin, switchMap } from 'rxjs';

@Component({
  selector: 'app-alunos-turma',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
   ],
  templateUrl: './alunos_turma.component.html',
  styleUrls: ['./alunos_turma.component.scss', '../../app.scss'],
})
export class AlunoTurmaComponent implements OnInit {
  public loaded = false;

  public alunos: Aluno[]  = [];
  public alunosFiltrados : Aluno[] = [];
  public criterios: Criterio[]  = [];
  public criteriosFiltrados : Criterio[] = [];
  private _filtroAlunos: string = '';
  private _filtroCriterios: string = '';
  public turma!: Turma;

  public get filtroAlunos() {
    return this._filtroAlunos
  }

  public set filtroAlunos(value : string) {
    this._filtroAlunos = value;
    this.alunosFiltrados = this.filtroAlunos ? this.filtrarAlunos(this.filtroAlunos) : this.alunos;
  }

  public get filtroCriterios() {
    return this._filtroCriterios
  }

  public set filtroCriterios(value : string) {
    this._filtroCriterios = value;
    this.criteriosFiltrados = this.filtroCriterios ? this.filtrarCriterios(this.filtroCriterios) : this.criterios;
  }

  public filtrarAlunos(filtrarPor: string): Aluno[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.alunos.filter(
      (aluno: { nome: string; }) => aluno.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  public filtrarCriterios(filtrarPor: string): Criterio[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.criterios.filter(
      (criterio: { nome: string; }) => criterio.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  Id: string = '';
  Nome: string = '';

  constructor(
    private alunoService: AlunoService,
    private turmaService: TurmaService,
    private criterioService: CriterioService,
    private route: ActivatedRoute
  ){}

  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        switchMap(params => {
          const turmaId = Number(params.get('id'));

          return forkJoin({
            alunos: this.alunoService.getAlunosTurma(turmaId),
            turma: this.turmaService.getTurmaId(turmaId),
            criterios: this.criterioService.getCriteriosTurma(turmaId)
          });
        })
      )
      .subscribe(({ alunos, turma, criterios }) => {
        this.alunos = alunos;
        this.alunosFiltrados = alunos;

        this.turma = turma;

        this.criterios = criterios;
        this.criteriosFiltrados = criterios;

        this.loaded = true;
      });
  }

  public getAlunosTurma (id: number): void{
    this.alunoService.getAlunosTurma(id).subscribe({
      next: (a: Aluno[]) =>
      {
        this.alunos = a;
        this.alunosFiltrados = this.alunos;
      },
      error: (e) => console.log(e)
    })
  }

  public getTurma (id: number): void{
    this.turmaService.getTurmaId(id).subscribe({
      next: (t: Turma) =>
      {
        this.turma = t;
      },
      error: (e) => console.log(e)
    })
  }

  public getCriteriosTurma (id: number): void{
    this.criterioService.getCriteriosTurma(id).subscribe({
      next: (c: Criterio[]) =>
      {
        this.criterios = c;
        this.criteriosFiltrados = this.criterios;
      },
      error: (e) => console.log(e)
    })
  }
}
