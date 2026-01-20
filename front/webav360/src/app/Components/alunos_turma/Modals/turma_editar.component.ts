import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Turma } from '../../../Models/Turma';


@Component({
  selector: 'app-turma-editar-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
  <div class="modal-header">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Turma {{ turmaEdit.cod }}</h4>
  </div>

  <div class="modal-body">
    <div class="input-group mb-3 row">
      <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Código: </span>
      <input type="text" class="form-control" [(ngModel)]="turmaEdit.cod" aria-label="Cod" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
    </div>
    <div class="input-group mb-3 row" >
      <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Nota Máxima: </span>
      <input type="decimal" class="form-control" [(ngModel)]="turmaEdit.notaMax" aria-label="Cod" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
    </div>
  </div>

  <div class="modal-footer">
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success" (click)="salvar()">Salvar</button>
  </div>
  `
})
export class TurmaEditarModalComponent implements OnInit {

  @Input() turma!: Turma;

  turmaEdit!: Turma;

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
    this.turmaEdit = { ...this.turma };
  }

  salvar(): void {
  console.log('Modal salvando:', this.turmaEdit);
    this.modal.close(this.turmaEdit);
  }
}
