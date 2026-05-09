import { ChangeDetectorRef, Component, DestroyRef, Inject, Input, OnInit, PLATFORM_ID } from '@angular/core';
import { DecimalPipe, isPlatformBrowser } from '@angular/common'
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';import {
	NgbAccordionButton,
	NgbAccordionDirective,
	NgbAccordionItem,
	NgbAccordionHeader,
	NgbAccordionToggle,
	NgbAccordionBody,
	NgbAccordionCollapse,
} from '@ng-bootstrap/ng-bootstrap/accordion';
import Swal from 'sweetalert2';

import { Turma } from '../../Models/Turma';
import { Aluno } from '../../Models/Aluno';
import { Criterio } from '../../Models/Criterio';
import { Grupo } from '../../Models/Grupo';
import { Sessao } from '../../Models/Sessao';

import { AlunoService } from '../../Service/Aluno.service';
import { TurmaService } from '../../Service/Turma.service';
import { CriterioService } from '../../Service/Criterio.service';
import { GrupoService } from '../../Service/Grupo.service';
import { SessaoService } from '../../Service/Sessao.service';
import { SessaoRealTime } from '../../Service/SessaoRealTime.service';
import { DashboardSessao } from '../../Models/Dashboard/DashboardSessao';
import { AuthService } from '../../auth/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-sessao-ativa',
  imports: [
		NgbAccordionButton,
		NgbAccordionDirective,
		NgbAccordionItem,
		NgbAccordionHeader,
		NgbAccordionToggle,
		NgbAccordionBody,
	  NgbAccordionCollapse,
    DecimalPipe
  ],
  templateUrl: './sessao_ativa.component.html',
  styleUrls: ['./sessao_ativa.component.scss','../../app.scss']
})
export class SessaoAtivaComponent implements OnInit {

  public alunos: Aluno[]  = [];
  public criterios: Criterio[]  = [];
  public grupos: Grupo[]  = [];
  public turma: Turma = ({id: 0, cod: '', notaMax: 0});
  public sessaoAtiva?: Sessao;
  public qrCode: string = '';
  public dashboard: DashboardSessao = ({
     sessaoId: 0
    , totalAlunos: 0
    , avaliaram: 0
    , pendentes: 0
    , mediaGeral: 0
    , totalNotas: 0
    , criterios: []
    , grupos: []
  });

  constructor(
    private alunoService: AlunoService,
    private turmaService: TurmaService,
    private criterioService: CriterioService,
    private grupoService: GrupoService,
    private sessaoService: SessaoService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private sessaoRealTime: SessaoRealTime,
    private authService: AuthService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ){}

  ngOnInit() {
    if (this.authService.isLogged()){
      const turmaId = Number(this.route.snapshot.paramMap.get('id'));

      this.loadData(turmaId);

      if (isPlatformBrowser(this.platformId) && this.sessaoAtiva) {
        const sessaoId = this.sessaoAtiva.id;
        this.sessaoRealTime.connect()?.then(() => {
          this.sessaoRealTime.acessarSessao(sessaoId);
        });

        this.sessaoRealTime.sessaoAtualizada$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id === turmaId) {
              this.loadData(turmaId);
            }
          });
    }}
  }

  public loadData(turmaId: number) {
    forkJoin({
      alunos: this.alunoService.getAlunosTurma(turmaId),
      criterios: this.criterioService.getCriteriosTurma(turmaId),
      grupos: this.grupoService.getGruposTurma(turmaId),
      turma: this.turmaService.getTurmaId(turmaId),
      sessaoAtiva: this.sessaoService.getSessaoAtivaTurma(turmaId),
    }).subscribe(({ alunos, criterios, grupos, turma, sessaoAtiva }) => {
      this.turma = turma;
      this.alunos = alunos;
      this.criterios = criterios;
      this.grupos = grupos;
      this.sessaoAtiva = sessaoAtiva;

      if (sessaoAtiva){
        const urlTree = this.router.createUrlTree([`/sessao-qrcode/${sessaoAtiva.id}`]);

        this.loadSessao(sessaoAtiva.id);

        window.open(this.router.serializeUrl(urlTree), '_blank')

        this.sessaoRealTime.connect()?.then(() => {
          this.sessaoRealTime.acessarSessao(sessaoAtiva.id);
        });

        this.sessaoRealTime.sessaoAtualizada$
          .subscribe(id => {
            if (id === sessaoAtiva.id) {
              this.loadSessao(sessaoAtiva.id);
            }
        });
      }

      this.cdr.detectChanges();
    });
  }

  public loadSessao(sessaoId: number){
    this.sessaoService.dashboardSessao(sessaoId).subscribe({
      next: (res) => {
        this.dashboard = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao buscar dashboard`
        });
      }
    });
  }

  public criaSessao() {
    this.sessaoService.postSessao({
      id: 0,
      turmaId: this.turma.id,
      turma: this.turma,
      dataInicio: new Date(),
      dataFim: new Date(),
      tokenPublico: '',
      ativo: true
    }).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Sucesso',
          text: `Período de Avaliação criado com sucesso!`
        });
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao criar Período de Avaliação`
        });
      }
    });
  }

  public encerrarSessao() {
    if (this.sessaoAtiva){
      this.sessaoService.putEncerraSessao(this.sessaoAtiva).subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Período de Avaliação Encerrado!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao encerrar Período de Avaliação`
          });
        }
      });
    }
  }

  public dashboardReset(sessaoId: number){
    this.sessaoService.dashboardResetSessao(sessaoId).subscribe({
      next: (res) => {
        this.dashboard = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao buscar dashboard`
        });
      }
    });
  }

  exportar(): void {
    this.sessaoService.getExportConsolidado(this.sessaoAtiva?.id ?? 0);
  }
}
