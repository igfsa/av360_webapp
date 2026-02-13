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
  indiceAtual = 0;

  constructor(
    private route: ActivatedRoute,
    private avaliacaoService: AvaliacaoService,
    private alunoService: AlunoService,
    private deviceService: DeviceService,
    private router: Router,
    private cdr: ChangeDetectorRef,
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
      next: (dto) => {this.dados = dto, this.grupos = dto.grupos, this.criterios = dto.criterios,
        console.log(this.grupos);
        console.log(this.criterios);
        console.log(this.dados);},
      error: (erro) => {console.log(erro); this.router.navigate(['/avaliacao/encerrada'])}
    });
    this.criterios.sort((a, b) => a.id - b.id);
    this.grupos.sort((a, b) => a.id - b.id);

    this.cdr.detectChanges();
  }

  public selecionarGrupo(grupo: any) {
    this.grupoSelecionado = grupo;
    this.alunoService.getAlunosGrupo(this.grupoSelecionado.id).subscribe(res => this.alunosGrupo = res );
    this.step = 2;
  }

  public validarAluno(){
    if (!this.grupoSelecionado)
      return;
    this.alunoService.getAlunoNomeIdGrupo(this.grupoSelecionado.id, this.nomeAluno).subscribe(
      aluno => {this.alunoLogado = aluno;
      this.carregarAvaliacao();
      console.log(this.avaliacoes);
    });
  }

  public carregarAvaliacao() {
    this.avaliacaoService.GeraNovaAvaliacaoEnvio(
      this.dados.sessaoId,
      this.grupoSelecionado.id,
      this.alunoLogado.id
    ).subscribe(data => {
      this.avaliacoes =  this.agruparAvaliacao(data.itens);
      this.indiceAtual = 0;
      this.step = 3;
    });
  }

  public proximo() {
    if (!this.avaliacaoValida(this.atual)) return;

    this.indiceAtual++;
    if (this.indiceAtual === this.avaliacoes.length) {
      this.step = 4;
    }
  }

  public async enviar() {
    const hash = await this.deviceService.getDeviceHash();
    const itens = this.desagruparAvaliacao(this.avaliacoes);

    this.avaliacaoService.PostAvaliacao({
      avaliadorId: this.alunoLogado.id,
      deviceHash: hash,
      grupoId: this.grupoSelecionado.id,
      itens: itens,
      sessaoId: this.dados.sessaoId
    }).subscribe(() => {
      this.step = 5;
    });
  }

  public editar(index: number) {
    this.indiceAtual = index;
    this.step = 3;
  }

  public avaliacaoValida(item: any): boolean {
    return item.criterios.every((c: any) => c.nota !== null);
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
