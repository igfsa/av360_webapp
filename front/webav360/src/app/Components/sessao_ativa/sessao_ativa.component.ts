import { ChangeDetectorRef, Component, DestroyRef, inject, Inject, Input, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { DecimalPipe, isPlatformBrowser } from '@angular/common'
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
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
import { ChartModule } from 'primeng/chart';

import { Turma } from '../../Models/Turma';
import { Sessao } from '../../Models/Sessao';

import { TurmaService } from '../../Service/Turma.service';
import { SessaoService } from '../../Service/Sessao.service';
import { SessaoRealTime } from '../../Service/SessaoRealTime.service';
import { DashboardSessao } from '../../Models/Dashboard/DashboardSessao';
import { AuthService } from '../../auth/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { DashboardSessaoComponent } from '../dashboard_sessao/dashboard_sessao.component';

@Component({
  selector: 'app-sessao-ativa',
  imports: [
    RouterLink,
    ChartModule,
    DashboardSessaoComponent
  ],
  templateUrl: './sessao_ativa.component.html',
  styleUrls: ['./sessao_ativa.component.scss','../../app.scss']
})
export class SessaoAtivaComponent implements OnInit, OnDestroy {

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
    , notaMax: 0
    , turmaCod: ``
    , criterios: []
    , grupos: []
  });
  private sessaoConectada?: number;

  constructor(
    private turmaService: TurmaService,
    private sessaoService: SessaoService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private sessaoRealTime: SessaoRealTime,
    private turmaRealTime: TurmaRealTime,
    private authService: AuthService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef,
  ){}

  ngOnInit() {
    if (this.authService.isLogged()){
      const turmaId = Number(this.route.snapshot.paramMap.get('id'));

      if (isPlatformBrowser(this.platformId)) {
        this.loadData(turmaId);

        this.turmaRealTime.connect()?.then(() => {
          this.turmaRealTime.acessarTurma(turmaId);
        });

        this.turmaRealTime.sessaoTurmaCriada$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id === turmaId) {
              this.loadData(turmaId);
            }
          });
    }}
  }

  ngOnDestroy(): void {
    this.turmaRealTime.disconnect();
    this.sessaoRealTime.disconnect();
  }

  public loadData(turmaId: number) {
    forkJoin({
      turma: this.turmaService.getTurmaId(turmaId),
      sessaoAtiva: this.sessaoService.getSessaoAtivaTurma(turmaId),
    }).subscribe(({turma, sessaoAtiva }) => {
      this.turma = turma;
      this.sessaoAtiva = sessaoAtiva;

      if (this.sessaoAtiva?.id){
        if (this.sessaoConectada !== this.sessaoAtiva.id){
          this.conectarSessao(sessaoAtiva.id);

          const urlTree = this.router.createUrlTree([`/sessao-qrcode/${sessaoAtiva.id}`]);
          window.open(this.router.serializeUrl(urlTree), '_blank')

          this.sessaoConectada = sessaoAtiva.id;
        }

        this.loadSessao(sessaoAtiva.id);
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
        Swal.mixin({
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
          }
        }).fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao buscar dashboard`
        });
      }
    });
  }

  private conectarSessao(sessaoId: number): void {

    this.sessaoRealTime.connect()?.then(() => {
      this.sessaoRealTime.acessarSessao(sessaoId);
    });

    this.sessaoRealTime.sessaoAtualizada$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(id => {
        if (id === sessaoId) {
          this.loadSessao(sessaoId);
        }
      });

    this.sessaoRealTime.sessaoFinalizada$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(id => {
        if (id === sessaoId) {
          this.sessaoRealTime.disconnect();
          this.loadData(this.turma.id);
          this.sessaoConectada = undefined;
        }
      });
  }

  public validaInicioSessao () {
    let podeIniciar: boolean = false;
    this.sessaoService.getValidaInicioSessao(this.turma.id)
      .subscribe({
        next: (res) => {
          podeIniciar = res.podeIniciar;
          if (!podeIniciar) {
            let texto: string = '<ul class="error-list">';
            res.mensagens.forEach(m => {
              texto += `<li>${m.tipo}: ${m.mensagem}. </li>`;
            })
            texto += `</ul>`;
            Swal.fire({
              icon: 'error',
              title: 'Erro',
              html: texto
            });
          }
          else if (podeIniciar && res.mensagens.length > 0) {
            let texto: string = '<ul class="warning-list">';
            res.mensagens.forEach(m => {
              texto += `<li>${m.tipo}: ${m.mensagem}. </li>`;
            })
            texto += `</ul>`;
            Swal.fire({
              icon: 'warning',
              title: 'Aviso',
              showDenyButton: true,
              confirmButtonText: "Iniciar Sessão",
              denyButtonText: `Cancelar`,
              html: `${texto}
                      <br> Deseja seguir com o início da sessão?`
            }).then((res) => {
              if (res.isConfirmed)
                this.criaSessao();
              if (res.isDenied)
                return
            });
          } else if (podeIniciar && res.mensagens.length == 0) {
            this.criaSessao();
          }
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao criar Período de Avaliação`
          });
        }
      })
  }

  private criaSessao() {
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
        Swal.mixin({
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
          }
        }).fire({
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

  public validarEncerrarSessao(){
    if (this.sessaoAtiva){
      this.sessaoService.getFaltamAvaliarSessao(this.sessaoAtiva.id).subscribe({
        next: (res) => {
          if (res.length != 0){
            let texto: string = '<ul class="warning-list">';
            res.forEach(a => {
              texto += `<li>${a.nome}. </li>`;
            })
            texto += `</ul>`;
            Swal.fire({
              icon: 'warning',
              title: 'Aviso',
              showDenyButton: true,
              confirmButtonText: "Encerrar Sessão",
              denyButtonText: `Cancelar`,
              html: `${res.length } alunos sem avaliar.
                      <br> ${texto}
                      <br> Deseja seguir com o fim da sessão?`
            }).then((res) => {
              if (res.isConfirmed)
                this.encerrarSessao();
              if (res.isDenied)
                return
            });
          } else {
            this.encerrarSessao();
          }

        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao encerrar Período de Avaliação`
          });
        }
      })
    }
  }

  private encerrarSessao() {
    this.sessaoService.putEncerraSessao(this.sessaoAtiva?.id ?? 0).subscribe({
      next: async (res) => {
        if (!res || res.size === 0){
          Swal.mixin({
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
              toast.onmouseenter = Swal.stopTimer;
              toast.onmouseleave = Swal.resumeTimer;
            }
          }).fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Período de Avaliação Encerrado!`
          });

          return
        }

        const result = await Swal.fire({
          icon: 'warning',
          title: 'Sessão encerrada com inconsistências',
          text: 'Deseja baixar o relatório de erros?',
          showCancelButton: true,
          confirmButtonText: 'Baixar relatório',
          cancelButtonText: 'Fechar'
        });

        if (result.isConfirmed) {

          const url = URL.createObjectURL(res);

          const a = document.createElement('a');
          a.href = url;
          a.download = `relatorio-erros-${this.sessaoAtiva?.id}.xlsx`;

          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);

          URL.revokeObjectURL(url);
        }

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

  public dashboardReset(sessaoId: number){
    this.sessaoService.dashboardResetSessao(sessaoId).subscribe({
      next: (res) => {
        this.dashboard = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        Swal.mixin({
          toast: true,
          position: "top-end",
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
          }
        }).fire({
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
