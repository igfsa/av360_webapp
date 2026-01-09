import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-turmas',
  imports: [ CommonModule ],
  templateUrl: './turmas.component.html',
  styleUrls: ['./turmas.component.scss', '../app.scss'],
})
export class TurmasComponent implements OnInit {

  public turmas: any  = [];
  public turmasFiltradas : any = [];

  Id: string = '';
  Cod: string = '';
  NotaMax: string = '';

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
