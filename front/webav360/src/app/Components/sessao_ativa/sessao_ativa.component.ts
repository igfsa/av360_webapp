import { ChangeDetectorRef, Component, DestroyRef, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common'
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

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
import { FRONT_URL } from '../../app.config';
import { LoadingComponent } from "../shared/loading/loading.component";
import { AlertService } from '../shared/alert/alert.service';
import { AlertHelper } from '../shared/alert/alert.helper';

@Component({
  selector: 'app-sessao-ativa',
  imports: [
    RouterLink,
    ChartModule,
    DashboardSessaoComponent,
    LoadingComponent
],
  templateUrl: './sessao_ativa.component.html',
  styleUrls: ['./sessao_ativa.component.scss','../../app.scss']
})
export class SessaoAtivaComponent implements OnInit, OnDestroy {

  public turma: Turma = ({id: 0, cod: '', notaMax: 0});
  public sessaoAtiva?: Sessao;
  public urlQrCode: string = '';
  public dashboard?: DashboardSessao;
  private sessaoConectada?: number;
  public loading: boolean = true;

  constructor(
    private turmaService: TurmaService,
    private sessaoService: SessaoService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private sessaoRealTime: SessaoRealTime,
    private turmaRealTime: TurmaRealTime,
    private authService: AuthService,
    private alert: AlertService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef,
    @Inject(FRONT_URL) public readonly frontURL: string
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

      this.loading = false;

      this.cdr.detectChanges();
    });
  }

  public loadSessao(sessaoId: number){
    this.sessaoService.dashboardSessao(sessaoId).subscribe({
      next: (res) => {
        this.urlQrCode = `${this.frontURL}/avaliacao/publica/${this.sessaoAtiva?.tokenPublico}`
        this.dashboard = structuredClone(res);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.alert.error(err.error?.message ?? `Erro ao buscar dashboard`);
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
            const texto = AlertHelper.createValidationList(
              res.mensagens,
              'error-list'
            );
            this.alert.error(texto);
          }
          else if (podeIniciar && res.mensagens.length > 0) {
            const texto = AlertHelper.createValidationList(
              res.mensagens,
              'warning-list'
            );
            this.alert.confirmWarning(
              'Aviso',
              `${texto}<br>Deseja seguir com o início da sessão?`
            ).then(result => {
              if(result.isConfirmed){
                this.criaSessao();
              }
            });
          } else if (podeIniciar && res.mensagens.length == 0) {
            this.criaSessao();
          }
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao criar Período de Avaliação`);
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
        this.alert.toastSuccess(`Período de Avaliação criado com sucesso!`);
      },
      error: (err) => {
        this.alert.error(err.error?.message ?? `Erro ao criar Período de Avaliação`);
      }
    });
  }

  public validarEncerrarSessao(){
    if (this.sessaoAtiva){
      this.sessaoService.getFaltamAvaliarSessao(this.sessaoAtiva.id).subscribe({
        next: (res) => {
          if (res.length != 0){
            const texto = AlertHelper.createList(
              res.map(a => a.nome),
              'warning-list'
            );
            this.alert.confirmWarning(
              'Aviso',
              `${res.length } alunos sem avaliar.
              <br> ${texto}
              <br> Deseja seguir com o fim da sessão?`
            ).then(result => {
              if(result.isConfirmed){
                this.encerrarSessao();
              }
            });
          } else {
            this.encerrarSessao();
          }

        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao encerrar Período de Avaliação`);
        }
      })
    }
  }

  private encerrarSessao() {
    this.sessaoService.putEncerraSessao(this.sessaoAtiva?.id ?? 0).subscribe({
      next: async (res) => {
        if (!res || res.size === 0){
          this.alert.toastSuccess(`Período de Avaliação finalizado com sucesso!`);
          return
        }

        const result = await this.alert.confirmDownload(
          'Sessão encerrada com inconsistências',
          'Deseja baixar o relatório de erros?'
        );

        if (result) {

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
        this.alert.error(err.error?.message ?? `Erro ao encerrar Período de Avaliação`);
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
        this.alert.error(err.error?.message ?? `Erro ao buscar dashboard`);
      }
    });
  }

  exportar(): void {
    this.sessaoService.getExportConsolidado(this.sessaoAtiva?.id ?? 0);
  }
}
