import { Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

import { Turma } from '../../../Models/Turma';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { Professor } from '../../../Models/Professor';
import { TurmaProfessorModalData } from '../../../Models/ModalData';


@Component({
  selector: 'app-turma-professor-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ModalLayoutComponent
],
  template: `

  <app-modal-layout
    (cancelar) = "ref.close()"
    (confirmar) = "confirmar()">
    <div class="modal-body flex-grow-1 overflow-auto" >
      <label>Selecione o professor: </label>
      <select
        class="form-control"
        [(ngModel)]="novoProfessor"
        >


        @for (professor of professores; track professor) {
          <option [ngValue]="professor">
            {{ professor.nome }}
          </option>
        }
        <option [ngValue]="null" >Remover Professor</option>
      </select>
    </div>
  </app-modal-layout>
  `
})
export class TurmaProfessorModalComponent implements OnInit {

  professor?: Professor | null;
  professores!: Professor[];
  turma!: Turma;
  novoProfessor!: Professor | null;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<TurmaProfessorModalData>,
  ) {}

  private get data(): TurmaProfessorModalData {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.turma = this.data.turma;
    this.professor = this.data.professor;
    this.professores = this.data.professores;

    this.novoProfessor = this.professor;
  }

  public confirmar(): void {
    this.ref.close(this.novoProfessor)
  }
}
