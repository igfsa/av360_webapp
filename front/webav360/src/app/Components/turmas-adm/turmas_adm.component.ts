import { ChangeDetectorRef, Component, DestroyRef, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

import { Turma } from '../../Models/Turma';
import { TurmaService } from '../../Service/Turma.service';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { AuthService } from '../../auth/auth.service';
import { ModalService } from '../shared/modal/modal.service';
import { LoadingComponent } from "../shared/loading/loading.component";
import { AlertService } from '../shared/alert/alert.service';
import { TableModule } from "primeng/table";
import { ProfessorService } from '../../Service/Professor.service';
import { convertTurmaAdm } from '../../Models/TurmaAdm';
import { Professor } from '../../Models/Professor';
import { TurmaProfessorModalComponent } from './modals/turma_editar.component';
import { TurmaProfessorModalData } from '../../Models/ModalData';
import { SessaoService } from '../../Service/Sessao.service';

@Component({
  selector: 'app-turmas-adm',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    LoadingComponent,
    TableModule
],
  templateUrl: './turmas_adm.component.html',
  styleUrls: ['./turmas_adm.component.scss', '../../app.scss'],
})
export class TurmasAdmComponent implements OnInit, OnDestroy {

  public turmas: Turma[] = [];
  public professores: Professor[] = [];
  public loading: boolean = true;
  protected readonly asTurmaAdm = convertTurmaAdm;

  id: string = '';
  cod: string = '';
  notaMax: string = '';

  constructor(
    private turmaService: TurmaService,
    private professorService: ProfessorService,
    private cdr: ChangeDetectorRef,
    private turmaRealTime: TurmaRealTime,
    private authService: AuthService,
    private modal: ModalService,
    private alert: AlertService,
    private sessaoService: SessaoService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ) { }

  ngOnInit() {
    if (this.authService.usuario())
    {
      this.getTurmas();

      if (isPlatformBrowser(this.platformId)) {
        this.turmaRealTime.connect();

        this.turmaRealTime.turmaAtualizada$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id) {
              this.getTurmas();
            }
          });

    }};
  }

  ngOnDestroy(): void {
    this.turmaRealTime.disconnect();
  }

  public getTurmas (): void{
    this.turmaService.getTurmasAdm().subscribe((turmas) => {
        this.turmas = turmas;

        this.loading = false;
        this.cdr.detectChanges();
      })
  }

  public editarTurmaProfessor (turma: Turma){
    this.sessaoService.getSessaoAtivaTurma(turma.id).subscribe({
      next: (res) => {
        if (res)
        {
          this.alert.error(`Turma contem sessão ativa. Não é possível alterar Professor`);
          return
        }
        console.log(res)
        this.professorService.getProfessores().subscribe(res => {
        const professorAtual = res.find(p => p.id == turma.professorId) ?? null;
        this.modal.open<TurmaProfessorModalData, Professor | null>(
          TurmaProfessorModalComponent,
          {
            professor: professorAtual,
            professores: res,
            turma: turma
          },
          { header: `Editar Turma` }
        ).subscribe((novoProfessor) => {
          if (novoProfessor === null)
          {
            this.alert.confirmWarning(
              'Aviso',
              `Professor ${professorAtual?.nome} será removido da turma ${turma.cod}`
            ).then(result => {
              if(result.isConfirmed){
                this.alterarProfessor(null, turma);
              }
            });
          } else if (!novoProfessor || novoProfessor === professorAtual) return;
          else
          {
            this.alert.confirmWarning(
              'Aviso',
              `Professor ${professorAtual?.nome ?? 'Sem Professor'} será alterado para Professor ${novoProfessor?.nome} na turma ${turma.cod}`
            ).then(result => {
              if(result.isConfirmed){
                this.alterarProfessor(novoProfessor, turma);
              }
            });
          }
        });
      })
    },
    error: (err) => {
      this.alert.error(err.error?.message ?? `Turma contem sessão ativa. Não é possível alterar Professor`);
    }
    })
  }

  public alterarProfessor (professorNovo: Professor | null, turma: Turma) {
    if (professorNovo) {
      this.turmaService.putAlterarProfessor(turma.id, professorNovo.id).subscribe({
        next: (c) => {
          this.alert.toastSuccess(`Professor ${c.professorNome} incluído na turma ${c.cod} com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao remover ${professorNovo.nome} da turma ${turma.cod}.`);
        }
      });
    } else {
      this.turmaService.putRemoverProfessor(turma.id).subscribe({
        next: (c) => {
          this.alert.toastSuccess(`Professor removido da turma ${c.cod} com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao remover professor da turma ${turma.cod}.`);
        }
      });
    }
  }

  public validaSessaoTurma (turma: Turma): boolean {
    var aux = false;
    this.sessaoService.getSessaoAtivaTurma(turma.id).subscribe(res => {
      if (!res) aux = true;
    })
    return aux
  }
}

