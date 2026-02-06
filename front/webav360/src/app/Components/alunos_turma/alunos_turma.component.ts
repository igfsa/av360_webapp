import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { catchError, forkJoin, map, ObservableInput, of  } from 'rxjs';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap/modal';
import {
	NgbAccordionButton,
	NgbAccordionDirective,
	NgbAccordionItem,
	NgbAccordionHeader,
	NgbAccordionToggle,
	NgbAccordionBody,
	NgbAccordionCollapse,
} from '@ng-bootstrap/ng-bootstrap/accordion';

import Swal from 'sweetalert2';

import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';
import { TurmaService } from '../../Service/Turma.service';
import { createEmptyTurma, Turma } from '../../Models/Turma';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';
import { TurmaEditarModalComponent } from './Modals/turma_editar.component';
import { TurmaCriterioModalComponent } from './Modals/turma_criterio_add.component';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { TurmaImportModalComponent } from '../turmas/Modals/turma_import.component';
import { ImportAlunos } from '../../Models/TurmaImport';
import { TurmaGrupoModalComponent } from './Modals/grupo_att.component';
import { GrupoService } from '../../Service/Grupo.service';
import { Grupo } from '../../Models/Grupo';
import { CriterioEditarModalComponent } from '../criterios/Modals/criterio_editar.component';
import { AlunoGrupoModalComponent } from './Modals/aluno_grupo_att.component';
import { AlunoGrupo } from '../../Models/AlunoGrupo';
import { Sessao } from '../../Models/Sessao';
import { SessaoService } from '../../Service/Sessao.service';

@Component({
  selector: 'app-alunos-turma',
  standalone: true,
  imports: [
		NgbAccordionButton,
		NgbAccordionDirective,
		NgbAccordionItem,
		NgbAccordionHeader,
		NgbAccordionToggle,
		NgbAccordionBody,
	  NgbAccordionCollapse,
    CommonModule,
    FormsModule,
    RouterLink,
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
  public criteriosFiltrados : Criterio[] = [];
  public grupos: Grupo[]  = [];
  public gruposFiltrados: Grupo[]  = [];
  private _filtroAlunos: string = '';
  private _filtroCriterios: string = '';
  private _filtroGrupos: string = '';
  public criterioIds: number[] = [];
  public turma: Turma = createEmptyTurma();
  public sessaoAtiva?: Sessao;

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

  public get filtroGrupos() {
    return this._filtroGrupos
  }

  public set filtroGrupos(value : string) {
    this._filtroGrupos = value;
    this.gruposFiltrados = this.filtroGrupos ? this.filtrarGrupos(this.filtroGrupos) : this.grupos;
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

  public filtrarGrupos(filtrarPor: string): Grupo[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.grupos.filter(
      (grupo: { nome: string; }) => grupo.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  Id: string = '';
  Nome: string = '';

  constructor(
    private alunoService: AlunoService,
    private turmaService: TurmaService,
    private criterioService: CriterioService,
    private grupoService: GrupoService,
    private sessaoService: SessaoService,
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
      grupos: this.grupoService.getGruposTurma(turmaId),
      turma: this.turmaService.getTurmaId(turmaId),
      sessao: this.sessaoService.GetSessaoAtivaTurma(turmaId)
    }).subscribe(({ alunos, criterios, grupos, turma, sessao }) => {
      this.turma = turma;

      this.alunos = alunos;
      this.alunosFiltrados = alunos;

      this.criterios = criterios;
      this.criteriosFiltrados = criterios;

      this.grupos = grupos;
      this.gruposFiltrados = grupos;

      this.sessaoAtiva = sessao;

      this.cdr.detectChanges();
    });
  }

  public editarTurma (): void{
    const ref = this.modalService.open(TurmaEditarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
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

  public alterarCriterioTurma (): void{
    forkJoin({
      criterios: this.criterioService.getCriterios(),
    }).subscribe(res => {
      const ref = this.modalService.open(TurmaCriterioModalComponent, {
        size: 'lg',
        backdrop: 'static',
        centered: true
      });

      ref.componentInstance.turma = this.turma;
      ref.componentInstance.criteriosTurma = this.criterios;
      ref.componentInstance.criterios = res.criterios;

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
    })
  }

  public alterarGruposTurma (): void{
    const ref = this.modalService.open(TurmaGrupoModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
    });

    ref.componentInstance.turma = this.turma;
    ref.componentInstance.gruposOrig = this.grupos;

    ref.result.then(({ add, edit }) => {

      const requests: ObservableInput<any>[] = [];

      if (add?.length) {
        add.forEach((g: Grupo) => {
          g.turmaId = this.turma.id;

          requests.push(
            this.grupoService.postGrupo(g).pipe(
              map(() => ({
                type: 'add',
                status: 'ok',
                grupo: g.nome
              })),
              catchError(err =>
                of({
                  type: 'add',
                  status: 'erro',
                  grupo: g.nome,
                  message: err?.error?.message || 'Erro desconhecido'
              }))
          ));
        });
      }

      if (edit?.length) {
        edit.forEach((g: Grupo) => {
          requests.push(
            this.grupoService.putGrupo(g).pipe(
              map(() => ({
                type: 'edit',
                status: 'ok',
                grupo: g.nome
              })),
              catchError(err =>
                of({
                  type: 'edit',
                  status: 'erro',
                  grupo: g.nome,
                  message: err?.error?.message || 'Erro desconhecido'
              }))
          ));
        });
      }

      if (!requests.length) return;

      forkJoin(requests).subscribe(results => {

        const addOk   = results.filter(r => r.type === 'add'  && r.status === 'ok').length;
        const addErro = results.filter(r => r.type === 'add'  && r.status === 'erro').length;
        const editOk  = results.filter(r => r.type === 'edit' && r.status === 'ok').length;
        const editErro= results.filter(r => r.type === 'edit' && r.status === 'erro').length;

        Swal.fire({
          icon: 'info',
          title: 'Info',
          html: `
            Equipes da turma ${this.turma.cod} processadas.<br><br>
            <b>Adicionadas</b>: ${addOk} sucesso, ${addErro} erro.<br>
            <b>Editadas</b>: ${editOk} sucesso, ${editErro} erro.
          `
        });
      });
    }).catch(() => {});
  }

  public alterarAlunosGrupo (grupo: Grupo): void{
    forkJoin({
      alunoGrupoCheckbox: this.grupoService.GetAlunoGruposCheckbox(grupo.id, this.turma.id),
    }).subscribe(res => {
      const ref = this.modalService.open(AlunoGrupoModalComponent, {
        size: 'lg',
        backdrop: 'static',
        centered: true
      });
      ref.componentInstance.turma = this.turma;
      ref.componentInstance.grupo = grupo;
      ref.componentInstance.alunosCheck = res.alunoGrupoCheckbox;
      ref.componentInstance.gruposTurma = this.grupos;
      ref.result.then((alunosGrupo: AlunoGrupo) => {
        if (!alunosGrupo) return;
        this.grupoService.putAtualizarGrupo({
          grupoId: alunosGrupo.grupoId,
          turmaId: alunosGrupo.turmaId,
          alunoIds: alunosGrupo.alunoIds
        }).subscribe(({
          next: () => {
            Swal.fire({
              icon: 'success',
              title: 'Sucesso',
              text: `Alunos do grupo ${grupo.nome} alterados com sucesso!`
            });
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Erro',
              text: err.error?.message ?? `Erro ao alterar alunos do grupo ${grupo.nome}`
            });
          }
        }))
      });
  })}


  public importAlunos(): void{
    const ref = this.modalService.open(TurmaImportModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
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
            html: `${imported.total} alunos da turma ${this.turma.cod} processados!<br>
                  ${imported.sucesso} importados com sucesso.<br>
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

  public editarCriterio (criterio: Criterio): void{
    const ref = this.modalService.open(CriterioEditarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
    });

    ref.componentInstance.criterio = criterio;

    ref.result.then((criterioEditado: Criterio) => {
      if (!criterioEditado) return;

      console.log(criterioEditado)

      this.criterioService.putCriterio(criterioEditado).subscribe({
        next: (c) => {
          criterio = c;
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Critério ${c.nome} editado com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao editar critério ${criterioEditado.nome}`
          });
        }
      });
    }).catch(() => {});
  }

}

