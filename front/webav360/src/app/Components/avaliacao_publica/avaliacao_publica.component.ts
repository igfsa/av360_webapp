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
import { forkJoin } from 'rxjs';
import { Turma } from '../../Models/Turma';
import { TurmaService } from '../../Service/Turma.service';
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
  turma!: Turma;
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
    private turmaService: TurmaService,
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
      error: (erro) => {console.log(erro); this.router.navigate(['/avaliacao/encerrada'])}
    });
  }

  public selecionarGrupo(grupo: any) {
    this.grupoSelecionado = grupo;
    this.step = 2;
  }

  public validarAluno(){
    if (!this.grupoSelecionado)
      return;
    this.alunoService.getAlunoNomeIdGrupo(this.grupoSelecionado.id, this.nomeAluno).subscribe(
      aluno => {
        this.alunoLogado = aluno;
        this.carregarAvaliacao();
    });
  }

  public async carregarAvaliacao() {
    const hash = await this.deviceService.getDeviceHash();
  //   forkJoin({
  //     avaliacao: this.avaliacaoService.GeraNovaAvaliacaoEnvio(
  //       this.dados.sessaoId,
  //       this.grupoSelecionado.id,
  //       this.alunoLogado.id,
  //       hash
  //     ),
  //     alunos: this.alunoService.getAlunosGrupo(this.grupoSelecionado.id),
  //     turma: this.turmaService.getTurmaId(this.dados.turmaId)
  //   }).subscribe({
  //     next: ({avaliacao, alunos, turma}) => {
  //       this.alunosGrupo = alunos;
  //       this.avaliacaoEnvio = avaliacao;
  //       console.log(this.avaliacaoEnvio)
  //       this.avaliacoes =  this.agruparAvaliacao(avaliacao.itens);
  //       this.turma = turma
  //       this.indiceAtual = 0;
  //       this.step = 3;
  //       this.carregandoAvaliacao = false;

  //       this.cdr.detectChanges();
  //     },
  //     error: (err) => {
  // console.log("ERROR EXECUTADO", err);
  //       Swal.fire({
  //         icon: 'error',
  //         title: 'Erro',
  //         text: err.error?.message ?? `${err}`
  //       });
  //     }
  //   });
  forkJoin({
  avaliacao: this.avaliacaoService.GeraNovaAvaliacaoEnvio(
    this.dados.sessaoId,
    this.grupoSelecionado.id,
    this.alunoLogado.id,
    hash
  ),
  alunos: this.alunoService.getAlunosGrupo(this.grupoSelecionado.id),
  turma: this.turmaService.getTurmaId(this.dados.turmaId)
})
.subscribe({
  next: (res) => {
    console.log("NEXT EXECUTADO", res);
  },
  error: (err) => {
    console.log("ERROR DO SUBSCRIBE EXECUTADO", err);
    alert("Erro caiu aqui");
  }
});}


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

    console.log(itens);

    this.avaliacaoService.PostAvaliacao(this.avaliacaoEnvio).subscribe(() => {
      this.step = 5;
    });
  }

  public editar(index: number) {
    this.indiceAtual = index;
    this.step = 3;
  }

  public avaliacaoValida(item: any): boolean {
    return item.criterios.every((c: any) => (c.nota >= 1 && c.nota <= this.turma.notaMax) || c.nota !== null);
  }

  private agruparAvaliacao(itens: AvaliacaoItem[]): AvaliacaoAgrupada[] {
    const mapa = new Map<number, AvaliacaoAgrupada>();
    itens.forEach(item => {
      if (!mapa.has(item.avaliadoId)) {
        mapa.set(item.avaliadoId, {
          avaliadoId: item.avaliadoId,
          avaliadoNome: this.alunosGrupo.find(a => a.id == item.avaliadoId)?.nome ?? '',
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

    console.log(Array.from(mapa.values()));
    return Array.from(mapa.values());
  }

  private desagruparAvaliacao(avaliacoes: AvaliacaoAgrupada[]): AvaliacaoItem[] {

    return avaliacoes.flatMap(aluno =>
      aluno.criterios.map(c => ({
        avaliadoId: aluno.avaliadoId,
        criterioId: c.criterioId,
        nota: c.nota
      }))
    );
  }
}
