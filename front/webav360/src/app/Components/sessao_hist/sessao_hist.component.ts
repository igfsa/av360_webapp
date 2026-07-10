import { ChangeDetectorRef, Component, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common'
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Turma } from '../../Models/Turma';
import { Sessao } from '../../Models/Sessao';

import { SessaoService } from '../../Service/Sessao.service';
import { DashboardSessao } from '../../Models/Dashboard/DashboardSessao';
import { AuthService } from '../../auth/auth.service';
import { DashboardSessaoComponent } from "../dashboard_sessao/dashboard_sessao.component";
import { ResultadoService } from '../../Service/Resultado.service';
import { LoadingComponent } from "../shared/loading/loading.component";
import { AlertService } from '../shared/alert/alert.service';

@Component({
  selector: 'app-sessao-hist',
  imports: [
    RouterLink,
    DashboardSessaoComponent,
    LoadingComponent
],
  templateUrl: './sessao_hist.component.html',
  styleUrls: ['./sessao_hist.component.scss','../../app.scss']
})
export class SessaoHistComponent implements OnInit, OnDestroy {

  public turma: Turma = ({id: 0, cod: '', notaMax: 0});
  public sessao: Sessao = ({
    id: 0
    , turmaId: 0
    , turma: this.turma
    , dataInicio: new Date
    , dataFim: new Date
    , tokenPublico: ``
    , ativo: false
  });
  public qrCode: string = '';
  public dashboard?: DashboardSessao;
  public loading: boolean = true;

  constructor(
    private sessaoService: SessaoService,
    private resultadoService: ResultadoService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
    private alert: AlertService,
    @Inject(PLATFORM_ID) private platformId: Object,
  ){}

  ngOnInit() {
    if (this.authService.isLogged()){
      const sessaoId = Number(this.route.snapshot.paramMap.get('id'));

      if (isPlatformBrowser(this.platformId)) {
        this.loadData(sessaoId);

    }}
  }

  ngOnDestroy(): void {
  }

  public loadData(sessaoId: number) {
    forkJoin({
      sessao: this.sessaoService.getSessaoId(sessaoId),
      dashboard: this.sessaoService.dashboardResultadoSessao(sessaoId),
    }).subscribe({
      next: ({sessao, dashboard}) => {
      this.sessao = sessao;

      this.dashboard = dashboard

      this.loading = false;

      this.cdr.detectChanges();
    },
    error: (err) => {
      this.alert.error(err.error?.message ?? `Erro ao buscar dashboard`);
    }}
  )};

  public exportar(): void {
    this.sessaoService.getExportConsolidado(this.sessao.id ?? 0);
  }

  public recalcularSessao(): void {
    this.resultadoService.putEncerraSessao(this.sessao.id).subscribe({
      next: async (res) => {
        this.loadData(this.sessao.id);

        if (!res || res.size === 0){
          this.alert.toastSuccess(`Dados de Avaliação Reprocessados!`);
          return
        }

        const result = await this.alert.confirmDownload(
          'Sessão reprocessada com inconsistências',
          'Deseja baixar o relatório de erros?'
        );

        if (result) {

          const url = URL.createObjectURL(res);

          const a = document.createElement('a');
          a.href = url;
          a.download = `relatorio-erros-${this.sessao?.id}.xlsx`;

          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);

          URL.revokeObjectURL(url);
        }

      },
      error: (err) => {
        this.alert.error(err.error?.message ?? `Erro ao reprocessar dados`);
      }
    });
  }
}
