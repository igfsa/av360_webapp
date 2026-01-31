import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AlunoService } from '../../Service/Aluno.service';
import { Aluno } from '../../Models/Aluno';
import { AvaliarModalComponent } from './modals/aluno_avaliar.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';

@Component({
  selector: 'app-alunos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
   ],
  templateUrl: './alunos.component.html',
  styleUrls: ['./alunos.component.scss', '../../app.scss'],
})
export class AlunosComponent implements OnInit {
	private modalService = inject(NgbModal);
  public loaded = false;

  public alunos: Aluno[]  = [];
  public alunosFiltrados : Aluno[] = [];
  private _filtroLista: string = '';
  public criterios: Criterio[] = [];

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

  constructor(
    private alunoService: AlunoService,
    private criterioService: CriterioService,
    private cdr: ChangeDetectorRef
  ){}

  ngOnInit() {
    this.getAlunos();
      this.criterioService.getCriterios().subscribe((criterios) => {
      this.criterios = criterios;
      this.loaded = true;
    })
  }

  public getAlunos (): void{
    this.alunoService.getAlunos().subscribe({
      next: (a: Aluno[]) =>
      {
        this.alunos = a;
        this.alunosFiltrados = this.alunos;

        this.cdr.detectChanges();
      },
      error: (e) => console.log(e)
    })
  }

  public avaliar(){
    const ref = this.modalService.open(AvaliarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
    });
    ref.componentInstance.criterios = this.criterios;
  }
}
