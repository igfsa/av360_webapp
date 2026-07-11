import { ChangeDetectorRef, Component, DestroyRef, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

import { Turma } from '../../Models/Turma';
import { TurmaService } from '../../Service/Turma.service';
import { TurmaCriarModalComponent } from './modals/turma_criar.component';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import { ImportAlunos } from '../../Models/TurmaImport';
import { TurmaImportModalComponent } from './modals/turma_import.component';
import { AuthService } from '../../auth/auth.service';
import { ModalService } from '../shared/modal/modal.service';
import { TurmaCriarModalData } from '../../Models/ModalData';
import { LoadingComponent } from "../shared/loading/loading.component";
import { AlertService } from '../shared/alert/alert.service';
import { TableModule } from "primeng/table";

@Component({
  selector: 'app-turmas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    LoadingComponent,
    TableModule
],
  templateUrl: './turmas.component.html',
  styleUrls: ['./turmas.component.scss', '../../app.scss'],
})
export class TurmasComponent implements OnInit, OnDestroy {

  public turmas: Turma[] = [];
  public loading: boolean = true;

  id: string = '';
  cod: string = '';
  notaMax: string = '';

  constructor(
    private turmaService: TurmaService,
    private cdr: ChangeDetectorRef,
    private turmaRealTime: TurmaRealTime,
    private authService: AuthService,
    private modal: ModalService,
    private alert: AlertService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ) { }

  ngOnInit() {
    if (this.authService.isLogged())
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
    this.turmaService.getTurmas().subscribe((turmas) => {
        this.turmas = turmas;

        this.loading = false;
        this.cdr.detectChanges();
      })
  }

  public adicionarTurma(): void{
    this.modal.open<null, TurmaCriarModalData>(
      TurmaCriarModalComponent,
      null ,
      { header: `Adicionar Aluno` }
    ).subscribe((res) => {
      const turma = res?.turma
      if (!turma) return;
      this.turmaService.postTurma(turma)
        .subscribe({
          next: turma => {
            this.alert.success(`Turma ${turma.cod} criada cm sucesso`)
            if (res.importarAlunos)
              this.ImportarAlunos(turma);
          },
          error: (err) => {
            this.alert.error(err.error?.message ?? `Erro ao criar turma ${turma.cod}`);
          }
        });
    });
  }

  public ImportarAlunos(turma: Turma){
    this.modal.open<Turma, ImportAlunos>(
      TurmaImportModalComponent,
      turma ,
      { header: `Importar Alunos` }
    ).subscribe((ImportAlunos) => {
      if (!ImportAlunos) return;

    this.turmaService.postImportarAlunos(ImportAlunos)
      .subscribe({
        next: imported => {
          this.alert.info(
            `${imported.total} alunos da turma ${turma.cod} processados!<br>
            ${imported.sucesso} importados com sucesso.<br>
            ${imported.falhas} com falha.`
          );
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao importar alunos para a turma ${turma.cod}`);
        }
      });
    });
  }
}

