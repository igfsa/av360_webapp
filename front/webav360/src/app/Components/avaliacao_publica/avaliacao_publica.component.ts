import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AvaliacaoPublica } from '../../Models/AvaliacaoPublica';
import { AvaliacaoService } from '../../Service/Avaliacao.service';
import { Grupo } from '../../Models/Grupo';
import { Criterio } from '../../Models/Criterio';
import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';
import { DeviceService } from '../../Helpers/hashGen';
import { AvaliacaoItem } from '../../Models/AvaliacaoItem';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AvaliacaoAgrupada } from '../../Models/AvaliacaoAgrupada';
import { AvaliacaoEnvio } from '../../Models/AvaliacaoEnvio';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-avaliacao_publica',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './avaliacao_publica.component.html',
  styleUrls: ['./avaliacao_publica.component.scss']
})

export class AvaliacaoPublicaComponent implements OnInit {
  step = 1;
  token!: string;
  dados!: AvaliacaoPublica;
  grupos: Grupo[] = [];
  alunosGrupo: Aluno[] = [];
  criterios: Criterio[] = [];
  grupoSelecionado!: Grupo;
  nomeAluno = '';
  alunoLogado!: Aluno;
  avaliacoes: AvaliacaoAgrupada[] = [];
  avaliacaoEnvio!: AvaliacaoEnvio;
  indiceAtual = 0;
  carregandoDados: boolean = true;
  carregandoAvaliacao: boolean = true;
  editarNotas: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private avaliacaoService: AvaliacaoService,
    private alunoService: AlunoService,
    private deviceService: DeviceService,
    private router: Router,
    public cdr: ChangeDetectorRef
  ) {}

  get atual() {
    return this.avaliacoes[this.indiceAtual];
  }

  ngOnInit(): void {
    this.token = this.route.snapshot.paramMap.get('token')!;
    this.carregarSessao();
  }

  public carregarSessao() {
    this.avaliacaoService.GetValidaSessaoChavePub(this.token).subscribe({
      next: (dto) => {
        this.dados = dto,
        this.grupos = [...dto.grupos].sort((a, b) => a.id - b.id);
        this.criterios = [...dto.criterios].sort((a, b) => a.id - b.id);
        this.carregandoDados = false;

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
        text: err.error.message ?? 'Ocorreu um erro não identificado'
      });
      this.router.navigate(['/avaliacao/encerrada'])}
    });
  }

  public selecionarGrupo(grupo: any) {
    this.grupoSelecionado = grupo;
    this.step = 2;
  }

  public validarAluno(){
    if (!this.grupoSelecionado)
      return;
    this.alunoService.getAlunoNomeIdGrupo(this.grupoSelecionado.id, this.nomeAluno).subscribe({
      next: (aluno) => {
        this.alunoLogado = aluno;
        this.carregarAvaliacao();
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
          text: err.error.message ?? 'Ocorreu um erro não identificado'
        });
      }
    });
  }

  public async carregarAvaliacao() {
    this.avaliacaoService.GeraNovaAvaliacaoEnvio(
      this.dados.sessaoId,
      this.grupoSelecionado.id,
      this.alunoLogado.id,
      await this.deviceService.getDeviceHash()
    ).subscribe({
      next: (data) => {
        this.avaliacoes =  this.agruparAvaliacao(data.itens);
        this.avaliacaoEnvio = {
          avaliadorId: data.avaliadorId,
          avaliador: data.avaliador,
          deviceHash: data.deviceHash,
          grupoId: data.grupoId,
          itens: [],
          sessaoId: data.sessaoId
        }
        this.indiceAtual = 0;
        this.step = 3;
        this.carregandoAvaliacao = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error.message ?? 'Ocorreu um erro não identificado'
        });
      }
    });
  }


  public proximo() {
    if (!this.avaliacaoValida(this.atual)) return;

    if(this.editarNotas) {
      this.step = 4;
      return
    }

    this.indiceAtual++;
    if (this.indiceAtual === this.avaliacoes.length) {
      this.editarNotas = true;
      this.step = 4;
    }
  }

  public enviar() {
    const itens = this.desagruparAvaliacao(this.avaliacoes);

    this.avaliacaoEnvio.itens = itens;

    this.avaliacaoService.PostAvaliacao(this.avaliacaoEnvio).subscribe({
      next: (imported) =>{
      this.step = 5;
      this.cdr.detectChanges();
        Swal.fire({
          icon: 'info',
          title: 'Sucesso',
          html: `${imported.total} notas processadas!<br>
                ${imported.sucesso} importados com sucesso.<br>
                ${imported.falhas} com falha.`
        });
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao importar Notas!`
        });
      }
    });
  }

  public editar(index: number) {
    this.indiceAtual = index;
    this.step = 3;
  }

  public avaliacaoValida(item: any): boolean {
    return item.criterios.every((c: any) => (c.nota >= 1 && c.nota <= this.dados.turma.notaMax));
  }

  private agruparAvaliacao(itens: AvaliacaoItem[]): AvaliacaoAgrupada[] {
    const mapa = new Map<number, AvaliacaoAgrupada>();
    itens.forEach(item => {
      if (!mapa.has(item.avaliadoId)) {
        mapa.set(item.avaliadoId, {
          avaliadoId: item.avaliadoId,
          avaliado: item.avaliado,
          criterios: []
        });
      }
      mapa.get(item.avaliadoId)!.criterios.push({
        criterioId: item.criterioId,
        criterioNome: this.criterios.find(c => c.id == item.criterioId)?.nome ?? '',
        nota: item.nota
      });
    });

    mapa.forEach(av => {
      av.criterios.sort((a, b) => a.criterioId - b.criterioId)
    })

    return Array.from(mapa.values());
  }

  private desagruparAvaliacao(avaliacoes: AvaliacaoAgrupada[]): AvaliacaoItem[] {

    return avaliacoes.flatMap(item =>
      item.criterios.map(c => ({
        avaliadoId: item.avaliadoId,
        avaliado: item.avaliado,
        criterioId: c.criterioId,
        nota: c.nota
      }))
    );
  }
}
