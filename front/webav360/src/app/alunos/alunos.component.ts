import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-alunos',
  imports: [
    CommonModule,
    FormsModule
   ],
  templateUrl: './alunos.component.html',
  styleUrls: ['./alunos.component.scss', '../app.scss'],
})
export class AlunosComponent implements OnInit {

  public alunos: any  = [];
  public alunosFiltrados : any = [];
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.alunosFiltrados = this.filtroLista ? this.filtrarAlunos(this.filtroLista) : this.alunos;
  }

  filtrarAlunos(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.alunos.filter(
      (aluno: { nome: string; }) => aluno.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  Id: string = '';
  Nome: string = '';

  constructor(private http :HttpClient) { }

  ngOnInit() {
    void this.getAlunos();
  }

  public getAlunos (): void{
    this.http.get('http://localhost:5074/api/Aluno/GetAllAlunos').subscribe({
      next: (a) =>
      {
        this.alunos = a;
        this.alunosFiltrados = this.alunos;
      },
      error: (e) => console.log(e)
    })
  }
}
