import { ChangeDetectorRef, Component, Inject, Input, OnInit, PLATFORM_ID, DestroyRef, OnDestroy } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { catchError, forkJoin, map, ObservableInput, of  } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { AccordionModule } from 'primeng/accordion';

import { LoadingComponent } from '../shared/loading/loading.component';

import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';
import { TurmaService } from '../../Service/Turma.service';
import { Turma } from '../../Models/Turma';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';
import { TurmaEditarModalComponent } from './modals/turma_editar.component';
import { TurmaCriterioModalComponent } from './modals/turma_criterio_add.component';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { TurmaImportModalComponent } from '../turmas/modals/turma_import.component';
import { ImportAlunos } from '../../Models/TurmaImport';
import { TurmaGrupoModalComponent } from './modals/grupo_att.component';
import { GrupoService } from '../../Service/Grupo.service';
import { Grupo } from '../../Models/Grupo';
import { CriterioEditarModalComponent } from '../criterios/modals/criterio_editar.component';
import { AlunoGrupoModalComponent } from './modals/aluno_grupo_add.component';
import { AlunoGrupo } from '../../Models/AlunoGrupo';
import { Sessao } from '../../Models/Sessao';
import { SessaoService } from '../../Service/Sessao.service';
import { AlunoGrupoNomes } from '../../Models/AlunoGrupoNomes';
import { AuthService } from '../../auth/auth.service';
import { AlunoTurmaAddModalComponent } from './modals/aluno_turma_add.component';
import { ModalService } from '../shared/modal/modal.service';
import { AlunoGrupoModalData, TurmaCriterioModalData, TurmaGrupoModalData, TurmaGrupoModalOut } from '../../Models/ModalData';
import { AlertService } from '../shared/alert/alert.service';
import { TableModule } from "primeng/table";

@Component({
  selector: 'app-alunos-turma',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    AccordionModule,
    LoadingComponent,
    TableModule
],
  providers: [],
  templateUrl: './alunos_turma.component.html',
  styleUrls: ['./alunos_turma.component.scss', '../../app.scss'],
})
export class AlunoTurmaComponent implements OnInit, OnDestroy {

  @Input() turmaEditar!: Turma;

  public alunos: Aluno[]  = [];
  public criterios: Criterio[]  = [];
  public grupos: Grupo[]  = [];
  public sessoes: Sessao[]  = [];
  public criterioIds: number[] = [];
  public turma: Turma = ({id: 0, cod: '', notaMax: 0});
  public sessaoAtiva?: Sessao;
  public alunoGrupo: AlunoGrupoNomes[] = [];
  public loading: boolean = true;

  public quantidadeAlunosGrupo(grupo : number): number{
    return this.alunoGrupo.filter(q => q.grupoId == grupo).length;
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
    private authService: AuthService,
    private modal: ModalService,
    private alert: AlertService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ){}

  ngOnInit(): void {
    const turmaId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.authService.isLogged()){
      this.loadData(turmaId);

      if (isPlatformBrowser(this.platformId)) {
        this.turmaRealTime.connect()?.then(() => {
          this.turmaRealTime.acessarTurma(turmaId);
        });

        this.turmaRealTime.turmaAtualizada$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id === turmaId) {
              this.loadData(turmaId);
            }
          });
    }}
  };

  ngOnDestroy(): void {
    this.turmaRealTime.disconnect();
  }

  public loadData(turmaId: number) {
    forkJoin({
      alunos: this.alunoService.getAlunosTurma(turmaId),
      criterios: this.criterioService.getCriteriosTurma(turmaId),
      grupos: this.grupoService.getGruposTurma(turmaId),
      sessoes: this.sessaoService.getSessoesTurmaId(turmaId),
      turma: this.turmaService.getTurmaId(turmaId),
      sessaoAtiva: this.sessaoService.getSessaoAtivaTurma(turmaId),
      alunoGrupo: this.alunoService.getAlunoGrupoNome(turmaId)
    }).subscribe(({ alunos, criterios, grupos, sessoes, turma, sessaoAtiva, alunoGrupo }) => {
      this.turma = turma;

      this.alunos = alunos;

      this.criterios = criterios;

      this.grupos = grupos;

      this.sessoes = sessoes.sort((a, b) => b.id - a.id);

      this.alunoGrupo = alunoGrupo;

      this.sessaoAtiva = sessaoAtiva;

      this.loading = false;

      this.cdr.detectChanges();
    });
  }

  public editarTurma (): void{

    this.modal.open<Turma, Turma>(
      TurmaEditarModalComponent,
       this.turma,
      { header: `Editar Turma` }
    ).subscribe((turmaEditada) => {
      if (!turmaEditada) return;

      this.turmaService.putTurma(turmaEditada).subscribe({
        next: (t) => {
          this.turma = t;
          this.alert.toastSuccess(`Turma ${turmaEditada.cod} salva com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao salvar turma ${turmaEditada.cod}`);
        }
      });
    });
  }

  public alterarCriterioTurma (): void{
    forkJoin({
      criterios: this.criterioService.getCriterios(),
    }).subscribe(res => {

      this.modal.open<TurmaCriterioModalData, number[]>(
        TurmaCriterioModalComponent,
        {
          turma: this.turma,
          criteriosTurma: this.criterios,
          criterios: res.criterios
        },
        { header: `Alterar Critérios` }
        ).subscribe((cts) => {
          if (!cts) return;
          this.turmaService.putCriterioTurma({
            turmaId: this.turma.id,
            criterioIds: cts
          }).subscribe(({
            next: () => {
              this.alert.toastSuccess(`Critérios da turma ${this.turma.cod} alterados com sucesso!`);
            },
            error: (err) => {
              this.alert.error(err.error?.message ?? `Erro ao alterar critérios da turma ${this.turma.cod}`);
            }
          }))
      });
    })
  }

  public alterarGruposTurma (): void{
    this.modal.open<TurmaGrupoModalData, TurmaGrupoModalOut>(
      TurmaGrupoModalComponent, {
        turma: this.turma,
        gruposOrig: this.grupos,
      },
      { header: `Alterar Grupos` }
    ).subscribe((resultado) => {
      if (!resultado) return;

      const requests: ObservableInput<any>[] = [];

      if (resultado.add?.length) {
        resultado.add.forEach((g: Grupo) => {
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

      if (resultado.edit?.length) {
        resultado.edit.forEach((g: Grupo) => {
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

        this.alert.info(
          `
            Equipes da turma ${this.turma.cod} processadas.<br><br>
            <b>Adicionadas</b>: ${addOk} sucesso, ${addErro} erro.<br>
            <b>Editadas</b>: ${editOk} sucesso, ${editErro} erro.
          `
        );
      });
    });
  }

  public alterarAlunosGrupo (grupo: Grupo): void{
    forkJoin({
      alunoGrupoCheckbox: this.grupoService.getAlunoGruposCheckbox(grupo.id, this.turma.id),
    }).subscribe(res => {
      this.modal.open<AlunoGrupoModalData, AlunoGrupo>(
        AlunoGrupoModalComponent,
        {
          turma: this.turma,
          grupo: grupo,
          alunosCheck: res.alunoGrupoCheckbox,
          gruposTurma: this.grupos
        },
        { header: `Modificar Grupo` }
      ).subscribe((alunosGrupo) => {
        if (!alunosGrupo) return;
        this.grupoService.putAtualizarGrupo({
          grupoId: alunosGrupo.grupoId,
          turmaId: alunosGrupo.turmaId,
          alunoIds: alunosGrupo.alunoIds
        }).subscribe(({
          next: () => {
            this.alert.toastSuccess(`Alunos da equipe ${grupo.nome} alterados com sucesso!`);
          },
          error: (err) => {
            this.alert.error(err.error?.message ?? `Erro ao alterar alunos da equipe ${grupo.nome}`);
          }
        }))
      });
  })}


  public importAlunos(): void{
    this.modal.open<Turma, ImportAlunos>(
      TurmaImportModalComponent,
      this.turma ,
      { header: `Importar Alunos` }
    ).subscribe((ImportAlunos) => {
      if (!ImportAlunos) return;

    this.turmaService.postImportarAlunos(ImportAlunos)
      .subscribe({
        next: imported => {
          this.alert.info(
            `${imported.total} alunos da turma ${this.turma.cod} processados!<br>
            ${imported.sucesso} importados com sucesso.<br>
            ${imported.falhas} com falha.`
          );
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao importar alunos para a turma ${this.turma.cod}`);
        }
      });
    });
  }

  public editarCriterio (criterio: Criterio): void{
    this.modal.open<Criterio, Criterio>(
      CriterioEditarModalComponent,
      criterio,
      { header: `Editar Critério` }
    ).subscribe((criterioEditado ) => {
      if (!criterioEditado) return;

      this.criterioService.putCriterio(criterioEditado).subscribe({
        next: (c) => {
          this.alert.toastSuccess(`Critério ${c.nome} editado com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao editar critério ${criterioEditado.nome}`);
        }
      });
    });
  }

  public buscaAlunoGrupoId(id: number){
    return this.alunoGrupo.find(ag => ag.alunoId == id)?.grupoNome;
  }

  public adicionarAluno(){
    this.modal.open<null, Aluno>(
      AlunoTurmaAddModalComponent,
      null ,
      { header: `Adicionar Aluno` }
    ).subscribe((aluno) => {
      if (!aluno)
        return

      this.alunoService.postAlunoTurma( this.turma.id, aluno).subscribe({
        next: (a) => {
          this.alert.toastSuccess(`Aluno ${a.nome} adicionado com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.message ?? `Erro ao adicionar aluno`);
        }
      })
    });
  }
}
