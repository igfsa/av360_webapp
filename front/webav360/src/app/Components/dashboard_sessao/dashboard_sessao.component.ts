import { DecimalPipe } from '@angular/common';
import { Component, Input, OnChanges, Type } from '@angular/core';

import { NgbAccordionBody, NgbAccordionButton, NgbAccordionCollapse, NgbAccordionDirective, NgbAccordionHeader, NgbAccordionItem, NgbAccordionToggle } from '@ng-bootstrap/ng-bootstrap';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { SelectButtonModule } from 'primeng/selectbutton';

import { DashboardSessao } from '../../Models/Dashboard/DashboardSessao';
import { GrupoDashboard } from '../../Models/Dashboard/GrupoDashboard';
import { FormsModule } from '@angular/forms';
import { DashboardViewMode } from '../../Models/Dashboard/DashboardViewMode.enum';

type GrupoDashboardView = GrupoDashboard & {
  chartMedia?: any;
  chartRadar?: any;
  chartMediaOption?: any;
  chartRadarOption?: any;
};

@Component({
  selector: 'app-dashboard_sessao',
  imports: [
		NgbAccordionButton,
		NgbAccordionDirective,
		NgbAccordionItem,
		NgbAccordionHeader,
		NgbAccordionToggle,
		NgbAccordionBody,
	  NgbAccordionCollapse,
    DecimalPipe,
    ChartModule,
    CardModule,
    SelectButtonModule,
    FormsModule,
  ],
  templateUrl: './dashboard_sessao.component.html',
  styleUrls: ['./dashboard_sessao.component.scss','../../app.scss']
})
export class DashboardSessaoComponent implements OnChanges {
  @Input({ required: true })
  dashboard!: DashboardSessao;

  DashboardViewMode = DashboardViewMode;

  public grupoDetailData: GrupoDashboardView[] = [];

  stateOptions = [
    {
      label: 'Monitoramento',
      value: DashboardViewMode.Monitoramento
    },
    {
      label: 'Análise',
      value: DashboardViewMode.Analise
    }
  ];

  public viewMode: DashboardViewMode = DashboardViewMode.Monitoramento;

  public dataGruposTurma: any;
  public dataCriteriosTurma: any;
  public donutDataTurma: any;

  public optionsGrupoTurma: any;
  public optionsCriterio: any;
  public radarOptions: any;

  public readonly cores = [
    '#42A5F5',
    '#66BB6A',
    '#FFA726',
    '#AB47BC',
    '#EC407A',
    '#26C6DA',
    '#FF7043',
    '#5C6BC0',
    '#9CCC65',
    '#8D6E63'
  ];

  constructor() { }

  ngOnChanges(): void {
    if (!this.dashboard) return;

    this.loadChart();
  }

  private loadChart() {

    this.optionsCriterio = {
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        },
        tooltip: {
          callbacks: {
            label: (context: any) => {

              const criterio =
                this.dashboard.criterios[context.dataIndex];

              return [
                `Média: ${criterio.mediaGlobal}`,
                `Total de notas: ${criterio.totalNotas}`
              ];
            }
          }
        }
      },
      scales: {
        x: {
          grid: {
            display: false
          }
        },
        y: {
          min: 0,
          max: this.dashboard.notaMax,
          grid: {
            display: false
          }
        }
      }
    };

    this.optionsGrupoTurma = {
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        },
        tooltip: {
          callbacks: {
            label: (context: any) => {

              const grupo =
                this.dashboard.grupos[context.dataIndex];

              return [
                `Média: ${grupo.media}`,
                `Total de notas: ${grupo.totalNotas}`
              ];
            }
          }
        }
      },
      scales: {
        x: {
          grid: {
            display: false
          }
        },
        y: {
          min: 0,
          max: this.dashboard.notaMax,
          grid: {
            display: false
          }
        }
      }
    };

    this.dataGruposTurma = {
      labels: this.dashboard.grupos.map(g => g.nome),
      datasets: [
        {
          type: 'bar',
          label: 'Média Grupo',
          data: this.dashboard.grupos.map(g => g.media),
          backgroundColor: this.dashboard.grupos.map(
            (_, index) =>
              this.hexToRgba( this.cores[index % this.cores.length], 0.6)
          )
        },
        {
          type: 'line',
          label: 'Média Global',
          data: this.dashboard.grupos.map(() => this.dashboard.mediaGeral),
          borderColor: '#7E57C2',
          borderDash: [8, 4],
          pointRadius: 0,
          fill: false
        }
      ]
    };

    this.dataCriteriosTurma = {
      labels: this.dashboard.criterios.map(c => c.nome),
      datasets: [
        {
          type: 'bar',
          label: 'Média Geral',
          data: this.dashboard.criterios.map(c => c.mediaGlobal),
          backgroundColor: this.dashboard.criterios.map(
            (_, index) =>
              this.hexToRgba( this.cores[index % this.cores.length], 0.6)
          )
        },
        {
          type: 'line',
          label: 'Média Global',
          data: this.dashboard.criterios.map(() => this.dashboard.mediaGeral),
          borderColor: '#7E57C2',
          borderDash: [8, 4],
          pointRadius: 0,
          fill: false
        }
      ]
    };

    this.donutDataTurma = {
      labels: ['Já Avaliararam', 'Pendentes'],
      datasets: [{
        data: [
          this.dashboard.avaliaram,
          this.dashboard.pendentes
        ]
      }]
    };

    this.grupoDetailData = this.dashboard.grupos.map(g => ({
      ...g,
      chartMedia: this.createMediaGrupoChart(g),
      chartMediaOption: this.createMediaGrupoOptiomChart(g),
      chartRadar: this.createRadarGrupoChart(g),
      chartRadarOption: this.createRadarGrupoOptiomChart(g)
    }));
  }

  createMediaGrupoOptiomChart(grupo: GrupoDashboard) {
    return {
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false
        },
        tooltip: {
          callbacks: {
            label: (context: any) => {

              const aluno =
                grupo.alunos[context.dataIndex];

              return [
                `Média: ${aluno.media}`,
                `Total de notas: ${aluno.totalNotas}`
              ];
            }
          }
        }
      },
      scales: {
        x: {
          grid: {
            display: false
          }
        },
        y: {
          min: 0,
          max: this.dashboard.notaMax,
          grid: {
            display: false
          }
        }
      }
    };
  }

  createRadarGrupoOptiomChart(grupo: GrupoDashboard) {
    return {
      maintainAspectRatio: false,
      plugins: {
        tooltip: {
          callbacks: {
            label: (context: any) => {

              const aluno =
                grupo.alunos[context.dataIndex];

              return [
                `Média: ${aluno.media}`,
                `Total de notas: ${aluno.totalNotas}`
              ];
            }
          }
        }
      },
      scales: {
        r: {
          beginAtZero: true,
          max: this.dashboard.notaMax
        }
      }
    };
  }

  createMediaGrupoChart(grupo: GrupoDashboard) {

    return {
      labels: grupo.alunos.map((a) => a.nome),

      datasets: [
        {
          type: 'bar',
          label: 'Média por Aluno',
          data: grupo.alunos.map(a => a.media),

          borderColor:this.dashboard.grupos.map(
            (_, index) => this.cores[index % this.cores.length]),
          backgroundColor: this.dashboard.grupos.map(
            (_, index) =>
              this.hexToRgba( this.cores[index % this.cores.length], 0.6)
          )
        },

        {
          type: 'line',
          label: 'Média do Grupo',
          data: grupo.alunos.map(() => grupo.media),
        }
      ]
    };
  }

  createRadarGrupoChart(grupo: GrupoDashboard) {

    const labels =
      grupo.alunos[0].criterioAluno.map(
        c => c.nome
      );

    const datasets =
      grupo.alunos.map((aluno, index) => ({

        label: aluno.nome,

        data:
          aluno.criterioAluno.map(c => c.media),

        borderColor:
          this.cores[index % this.cores.length],

        backgroundColor:
          this.hexToRgba(
            this.cores[index % this.cores.length],
            0.2
          )
      }));

    return {
      labels,
      datasets
    };
  }

  hexToRgba(hex: string, alpha: number = 1): string {
    let cleanHex = hex.replace('#', '');

    if (cleanHex.length === 3) {
      cleanHex = cleanHex.split('').map(char => char + char).join('');
    }

    const r = parseInt(cleanHex.substring(0, 2), 16);
    const g = parseInt(cleanHex.substring(2, 4), 16);
    const b = parseInt(cleanHex.substring(4, 6), 16);

    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
  }


}
