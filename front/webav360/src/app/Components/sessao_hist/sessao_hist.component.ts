import { ChangeDetectorRef, Component, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common'
import { ActivatedRoute, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';

import { Turma } from '../../Models/Turma';
import { Sessao } from '../../Models/Sessao';

import { SessaoService } from '../../Service/Sessao.service';
import { DashboardSessao } from '../../Models/Dashboard/DashboardSessao';
import { AuthService } from '../../auth/auth.service';
import { DashboardSessaoComponent } from "../dashboard_sessao/dashboard_sessao.component";
import { ResultadoService } from '../../Service/Resultado.service';

@Component({
  selector: 'app-sessao-hist',
  imports: [
    RouterLink,
    DashboardSessaoComponent
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
  public dashboard: DashboardSessao = ({
     sessaoId: 0
    , totalAlunos: 0
    , avaliaram: 0
    , pendentes: 0
    , mediaGeral: 0
    , totalNotas: 0
    , notaMax: 0
    , turmaCod: ``
    , inconsistencia: false
    , criterios: []
    , grupos: []
  });

  constructor(
    private sessaoService: SessaoService,
    private resultadoService: ResultadoService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private authService: AuthService,
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
            text: `Dados de Avaliação Reprocessados!`
          });

          return
        }

        const result = await Swal.fire({
          icon: 'warning',
          title: 'Sessão reprocessada com inconsistências',
          text: 'Deseja baixar o relatório de erros?',
          showCancelButton: true,
          confirmButtonText: 'Baixar relatório',
          cancelButtonText: 'Fechar'
        });

        if (result.isConfirmed) {

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
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao reprocessar dados`
        });
      }
    });
  }
}
