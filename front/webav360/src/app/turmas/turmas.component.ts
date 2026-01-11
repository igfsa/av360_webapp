import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-turmas',
  imports: [
    CommonModule,
    FormsModule
   ],
  templateUrl: './turmas.component.html',
  styleUrls: ['./turmas.component.scss', '../app.scss'],
})
export class TurmasComponent implements OnInit {

  public turmas: any  = [];
  public turmasFiltradas : any = [];
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.turmasFiltradas = this.filtroLista ? this.filtrarTurmas(this.filtroLista) : this.turmas;
  }

  filtrarTurmas(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.turmas.filter(
      (turma: { cod: string; }) => turma.cod.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  id: string = '';
  cod: string = '';
  notaMax: string = '';

  constructor(private http :HttpClient) { }

  ngOnInit() {
    void this.getTurmas();
  }

  public getTurmas (): void{
    this.http.get('http://localhost:5074/api/Turma/GetAllTurmas').subscribe({
      next: (t) =>
      {
        this.turmas = t;
        this.turmasFiltradas = this.turmas;
      },
      error: (e) => console.log(e)
    })
  }
}
