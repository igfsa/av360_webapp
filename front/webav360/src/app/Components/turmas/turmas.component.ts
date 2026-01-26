import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
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
  ) { }

  ngOnInit() {
    this.getTurmas();
    this.turmaRealTime.connect();

    this.turmaRealTime.turmaAtualizada$
      .subscribe(id => {
        if (id) {
          this.getTurmas();
        }
      });
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
      backdrop: 'static'
    });
    var turmaRes: Turma;
    ref.result.then(({Turma, ImportAlunos}) => {
      if (!Turma) return;
      this.turmaService.postTurma(Turma)
        .subscribe({
          next: turma => {
            turmaRes = turma;
            Swal.fire({
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
