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

  public adicionarTurma (): void{
    const ref = this.modalService.open(TurmaCriarModalComponent, {
      size: 'lg',
      backdrop: 'static'
    });

    ref.result.then((turmaEditada: Turma) => {
      if (!turmaEditada) return;

    this.turmaService.postTurma(turmaEditada)
      .subscribe({
        next: turma => {
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Turma ${turma.cod} criada com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao criar turma ${turmaEditada.cod}`
          });
        }
      });
    }).catch(() => {});
  }
}
