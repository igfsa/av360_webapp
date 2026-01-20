import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Turma } from '../../Models/Turma';
import { TurmaService } from '../../Service/Turma.service';

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
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    void this.getTurmas();
  }

  public getTurmas (): void{
    this.turmaService.getTurmas().subscribe({
      next: (t: Turma[]) =>
      {
        this.turmas = t;
        this.turmasFiltradas = this.turmas;

        this.cdr.detectChanges();
      },
      error: (e) => console.log(e)
    })
  }
}
