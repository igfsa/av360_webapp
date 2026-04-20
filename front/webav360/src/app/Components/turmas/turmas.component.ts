import { ChangeDetectorRef, Component, inject, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Turma } from '../../Models/Turma';
import { TurmaService } from '../../Service/Turma.service';
import { TurmaCriarModalComponent } from './Modals/turma_criar.component';
import { TurmaRealTime } from '../../Service/TurmaRealTime.service';
import Swal from 'sweetalert2';
import { ImportAlunos } from '../../Models/TurmaImport';
import { TurmaImportModalComponent } from './Modals/turma_import.component';
import { AuthService } from '../../auth/auth.service';
import { Console } from 'console';

@Component({
  selector: 'app-turmas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
   ],
  templateUrl: './turmas.component.html',
  styleUrls: ['./turmas.component.scss', '../../app.scss'],
})
export class TurmasComponent implements OnInit {

	private modalService = inject(NgbModal);
  private platformId = inject(PLATFORM_ID);

  public turmas: Turma[] = [];
  public turmasFiltradas : Turma[] = [];
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.turmasFiltradas = this.filtroLista ? this.filtrarTurmas(this.filtroLista) : this.turmas;
  }

  public filtrarTurmas(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.turmas.filter(
      (turma: { cod: string; }) => turma.cod.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  id: string = '';
  cod: string = '';
  notaMax: string = '';

  constructor(
    private turmaService: TurmaService,
    private cdr: ChangeDetectorRef,
    private turmaRealTime: TurmaRealTime,
    private authService: AuthService
  ) { }

  ngOnInit() {
    if (this.authService.isLogged())
    {
      this.getTurmas();

      if (isPlatformBrowser(this.platformId)) {
        this.turmaRealTime.connect();

        this.turmaRealTime.turmaAtualizada$
          .pipe(takeUntilDestroyed())
          .subscribe(id => {
            if (id) {
              this.getTurmas();
            }
          });

    }};
  }

  public getTurmas (): void{
    this.turmaService.getTurmas().subscribe((turmas) => {
        this.turmas = turmas;
        this.turmasFiltradas = this.turmas;

        this.cdr.detectChanges();
      })
  }

  public adicionarTurma(): void{
    const ref = this.modalService.open(TurmaCriarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true,
      fullscreen: true,
      scrollable: true
    });
    ref.result.then(({Turma, ImportAlunos}) => {
      if (!Turma) return;
      this.turmaService.postTurma(Turma)
        .subscribe({
          next: turma => {
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
              text: `Turma ${turma.cod} criada com sucesso!`
            }).then(() => {
              if (ImportAlunos)
                this.ImportarAlunos(turma);
            })
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Erro',
              text: err.error?.message ?? `Erro ao criar turma ${Turma.cod}`
            });
          }
        });
    }).catch(() => {});
  }

  public ImportarAlunos(turma: Turma){
    const refImport = this.modalService.open(TurmaImportModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true,
      fullscreen: true,
      scrollable: true
    });
    refImport.componentInstance.turma = turma;
    refImport.result.then((ImportAlunos: ImportAlunos) => {
      if (!ImportAlunos) return;
      ImportAlunos.turmaId = turma.id;
      this.turmaService.postImportarAlunos(ImportAlunos)
        .subscribe({
          next: imported => {
            Swal.fire({
              icon: 'info',
              title: 'Sucesso',
              text: `${imported.total} alunos da turma ${turma.cod} processados!
                    ${imported.sucesso} importados com sucesso.
                    ${imported.falhas} com falha.`
            });
          },
          error: (err) => {
            Swal.fire({
              icon: 'error',
              title: 'Erro',
              text: err.error?.message ?? `Erro ao importar alunos para a turma ${turma.cod}`
            });
          }
        });
      }).catch(() => {});
  }
}
function takeUntilDestroyed(): import("rxjs").OperatorFunction<number, unknown> {
  throw new Error('Function not implemented.');
}

