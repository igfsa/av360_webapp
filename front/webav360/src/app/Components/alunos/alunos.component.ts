import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';

@Component({
  selector: 'app-alunos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
   ],
  templateUrl: './alunos.component.html',
  styleUrls: ['./alunos.component.scss', '../../app.scss'],
})
export class AlunosComponent implements OnInit {
  public loaded = false;

  public alunos: Aluno[]  = [];
  public alunosFiltrados : Aluno[] = [];
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.alunosFiltrados = this.filtroLista ? this.filtrarAlunos(this.filtroLista) : this.alunos;
  }

  public filtrarAlunos(filtrarPor: string): Aluno[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.alunos.filter(
      (aluno: { nome: string; }) => aluno.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  Id: string = '';
  Nome: string = '';

  constructor(private alunoService: AlunoService){}

  ngOnInit() {
    void this.getAlunos();
    this.loaded = true;
  }

  public getAlunos (): void{
    this.alunoService.getAlunos().subscribe({
      next: (a: Aluno[]) =>
      {
        this.alunos = a;
        this.alunosFiltrados = this.alunos;
      },
      error: (e) => console.log(e)
    })
  }
}
