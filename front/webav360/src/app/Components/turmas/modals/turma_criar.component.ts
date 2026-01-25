import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { createEmptyTurma, Turma } from '../../../Models/Turma';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
  <div class="modal-header">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Nova Turma</h4>
  </div>

  <div class="modal-body">
    <div class="input-group mb-3 row">
      <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Código: </span>
      <input type="text" class="form-control" [(ngModel)]="novaTurma.cod" aria-label="Cod" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
    </div>
    <div class="input-group mb-3 row" >
      <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Nota Máxima: </span>
      <input type="decimal" class="form-control" [(ngModel)]="novaTurma.notaMax" aria-label="Nota Máxima" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
    </div>
  </div>

  <div class="modal-footer">
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success" (click)="salvar()">Salvar</button>
  </div>
  `
})
export class TurmaCriarModalComponent implements OnInit {
  public novaTurma!: Turma;

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
    this.novaTurma = createEmptyTurma();
  }

  salvar(): void {
    console.log(this.novaTurma)
    this.modal.close(this.novaTurma);
  }
}
