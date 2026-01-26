import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { forkJoin  } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap/modal';
import Swal from 'sweetalert2';

import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';
import { TurmaService } from '../../Service/Turma.service';
import { Turma } from '../../Models/Turma';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';
import { TurmaEditarModalComponent } from './Modals/turma_editar.component';
import { TurmaCriterioModalComponent } from './Modals/turma_criterio_add.component';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { TurmaImportModalComponent } from '../turmas/Modals/turma_import.component';
import { ImportAlunos } from '../../Models/TurmaImport';

@Component({
  selector: 'app-alunos-turma',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
   ],
  templateUrl: './alunos_turma.component.html',
  styleUrls: ['./alunos_turma.component.scss', '../../app.scss'],
})
export class AlunoTurmaComponent implements OnInit {

	private modalService = inject(NgbModal);
  @Input() turmaEditar!: Turma;

  public alunos: Aluno[]  = [];
  public alunosFiltrados : Aluno[] = [];
  public criterios: Criterio[]  = [];
  public criteriosGlobais: Criterio[]  = [];
  public criteriosFiltrados : Criterio[] = [];
  private _filtroAlunos: string = '';
  private _filtroCriterios: string = '';
  public turma!: Turma;
  public criterioIds: number[] = [];

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
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private turmaRealTime: TurmaRealTime,
  ){}

  ngOnInit(): void {
    const turmaId = Number(this.route.snapshot.paramMap.get('id'));

    this.loadData(turmaId);

    this.turmaRealTime.connect()?.then(() => {
      this.turmaRealTime.acessarTurma(turmaId);
    });

    this.turmaRealTime.turmaAtualizada$
      .subscribe(id => {
        if (id === turmaId) {
          this.loadData(turmaId);
        }
      });
  }

  public loadData(turmaId: number) {
    forkJoin({
      alunos: this.alunoService.getAlunosTurma(turmaId),
      criterios: this.criterioService.getCriteriosTurma(turmaId),
      turma: this.turmaService.getTurmaId(turmaId),
      criteriosGlobais: this.criterioService.getCriterios()
    }).subscribe(({ alunos, criterios, turma, criteriosGlobais }) => {
      this.turma = turma;

      this.alunos = alunos;
      this.alunosFiltrados = alunos;

      this.criterios = criterios;
      this.criteriosFiltrados = criterios;

      this.criteriosGlobais = criteriosGlobais;

      this.cdr.detectChanges();
    });
  }

  public editarTurma (): void{
    const ref = this.modalService.open(TurmaEditarModalComponent, {
      size: 'lg',
      backdrop: 'static'
    });
    ref.componentInstance.turma = this.turma;

    ref.result.then((turmaEditada: Turma) => {
      if (!turmaEditada) return;

      this.turmaService.putTurma(turmaEditada).subscribe({
        next: (t) => {
          this.turma = t;
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Turma ${turmaEditada.cod} salva com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao salvar turma ${turmaEditada.cod}`
          });
        }
      });
    }).catch(() => {});
  }

  public adicionarCriterioTurma (): void{
    const ref = this.modalService.open(TurmaCriterioModalComponent, {
      size: 'lg',
      backdrop: 'static'
    });

    ref.componentInstance.turma = this.turma;
    ref.componentInstance.criteriosTurma = this.criterios;
    ref.componentInstance.criterios = this.criteriosGlobais;

    ref.result.then((cts: number[]) =>{
      if (!cts) return;

      this.turmaService.putCriterioTurma({
        turmaId: this.turma.id,
        criterioIds: cts
      }).subscribe(({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Critérios da turma ${this.turma.cod} alterados com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao alterar critérios da turma ${this.turma.cod}`
          });
        }
      }))
    });
  }

  public importAlunos(): void{
    const ref = this.modalService.open(TurmaImportModalComponent, {
      size: 'lg',
      backdrop: 'static'
    });

    ref.componentInstance.turma = this.turma;

    ref.result.then((ImportAlunos: ImportAlunos) => {
      if (!ImportAlunos) return;

    this.turmaService.postImportarAlunos(ImportAlunos)
      .subscribe({
        next: imported => {
          Swal.fire({
            icon: 'info',
            title: 'Sucesso',
            text: `${imported.total} alunos da turma ${this.turma.cod} processados!
                  ${imported.sucesso} importados com sucesso.
                  ${imported.falhas} com falha.`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao importar alunos para a turma ${this.turma.cod}`
          });
        }
      });
    }).catch(() => {});
  }
}
