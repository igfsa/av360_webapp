import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';

import { createEmptyTurma, Turma } from '../../Models/Turma';
import { Aluno } from '../../Models/Aluno';
import { Criterio } from '../../Models/Criterio';
import { Grupo } from '../../Models/Grupo';

import { AlunoService } from '../../Service/Aluno.service';
import { TurmaService } from '../../Service/Turma.service';
import { CriterioService } from '../../Service/Criterio.service';
import { GrupoService } from '../../Service/Grupo.service';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { SessaoService } from '../../Service/Sessao.service';
import { Sessao } from '../../Models/Sessao';
import { formatDate } from '@angular/common';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-sessao',
  templateUrl: './sessao.component.html',
  styleUrls: ['./sessao.component.scss', '../../app.scss']
})
export class SessaoComponent implements OnInit {

	private modalService = inject(NgbModal);
  @Input() turmaEditar!: Turma;

  public alunos: Aluno[]  = [];
  public criterios: Criterio[]  = [];
  public grupos: Grupo[]  = [];
  public turma: Turma = createEmptyTurma();
  public sessaoAtiva?: Sessao;
  public qrCode: string = '';

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

  ngOnInit() {
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
      sessaoAtiva: this.sessaoService.GetSessaoAtivaTurma(turmaId),
    }).subscribe(({ alunos, criterios, grupos, turma, sessaoAtiva }) => {
      this.turma = turma;
      this.alunos = alunos;
      this.criterios = criterios;
      this.grupos = grupos;
      this.sessaoAtiva = sessaoAtiva;

      if (sessaoAtiva){
        this.qrCode = `http://localhost:5074/api/Sessao/GetQrCode/${sessaoAtiva.id}/qrcode`
      }
      this.cdr.detectChanges();
    });
  }

  public criaSessao() {
    this.sessaoService.postSessao({
      id: 0,
      turmaId: this.turma.id,
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

}
